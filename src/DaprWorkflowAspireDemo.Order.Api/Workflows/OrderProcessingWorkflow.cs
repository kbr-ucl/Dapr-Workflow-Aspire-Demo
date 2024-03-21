using CrossCut.Messages;
using CrossCut.Messages.OrderProcessingWorkflowModels.Warehouse;
using Dapr.Workflow;
using DaprWorkflowAspireDemo.Order.Api.Activities;

namespace DaprWorkflowAspireDemo.Order.Api.Workflows;

// NOTE: https://github.com/dapr/dotnet-sdk/blob/master/examples/Workflow/WorkflowConsoleApp/Workflows/OrderProcessingWorkflow.cs
public class OrderProcessingWorkflow : Workflow<OrderPayload, OrderResult>
{
    private readonly WorkflowTaskOptions _defaultActivityRetryOptions = new()
    {
        // NOTE: Beware that changing the number of retries is a breaking change for existing workflows.
        RetryPolicy = new WorkflowRetryPolicy(
            3,
            TimeSpan.FromSeconds(5))
    };

    public override async Task<OrderResult> RunAsync(WorkflowContext context, OrderPayload order)
    {
        var orderId = context.InstanceId;

        // Notify the user that an order has come through
        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notification($"Received order {orderId} for {order.Quantity} {order.ItemName} at ${order.TotalCost}"));

        // Determine if there is enough of the item available for purchase by checking the inventory
        var result = await context.CallActivityAsync<InventoryItemReservationResult>(
            nameof(ReserveInventoryActivity),
            new InventoryItemCreateReservationRequest(orderId,order.ItemId, order.ItemName, order.Quantity),
            _defaultActivityRetryOptions);

        // If there is insufficient inventory, fail and let the user know 
        if (!result.Success)
        {
            // End the workflow here since we don't have sufficient inventory
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Insufficient inventory for {order.ItemName}"));
            return new OrderResult(false);
        }

        // Require orders over a certain threshold to be approved
        if (order.TotalCost > 50000)
        {
            // Request manager approval for the order
            await context.CallActivityAsync(nameof(RequestApprovalActivity), order);

            try
            {
                // Pause and wait for a manager to approve the order
                context.SetCustomStatus("Waiting for approval");
                var approvalResult = await context.WaitForExternalEventAsync<ApprovalResult>(
                    "ManagerApproval",
                    TimeSpan.FromSeconds(30));
                context.SetCustomStatus($"Approval result: {approvalResult}");
                if (approvalResult == ApprovalResult.Rejected)
                {
                    // The order was rejected, end the workflow here
                    await context.CallActivityAsync(
                        nameof(NotifyActivity),
                        new Notification("Order was rejected by approver"));
                    return new OrderResult(false);
                }
            }
            catch (TaskCanceledException)
            {
                // An approval timeout results in automatic order cancellation
                await context.CallActivityAsync(
                    nameof(NotifyActivity),
                    new Notification("Cancelling order because it didn't receive an approval"));
                return new OrderResult(false);
            }
        }

        // There is enough inventory available so the user can purchase the item(s). Process their payment
        await context.CallActivityAsync(
            nameof(ProcessPaymentActivity),
            new PaymentRequest(orderId, order.ItemName, order.Quantity, order.TotalCost),
            _defaultActivityRetryOptions);

        try
        {
            // There is enough inventory available so the user can purchase the item(s). Process their payment
            await context.CallActivityAsync(
                nameof(UpdateInventoryActivity),
                new PaymentRequest(orderId, order.ItemName, order.Quantity, order.TotalCost),
                _defaultActivityRetryOptions);
        }
        catch (WorkflowTaskFailedException e)
        {
            // Let them know their payment processing failed
            await context.CallActivityAsync(
                nameof(NotifyActivity),
                new Notification($"Order {orderId} Failed! Details: {e.FailureDetails.ErrorMessage}"));
            return new OrderResult(false);
        }

        // Let them know their payment was processed
        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new Notification($"Order {orderId} has completed!"));

        // End the workflow with a success result
        return new OrderResult(true);
    }
}