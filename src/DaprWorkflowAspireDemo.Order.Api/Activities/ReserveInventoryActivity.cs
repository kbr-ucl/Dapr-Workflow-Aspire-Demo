using CrossCut.DaprConsts;
using CrossCut.Messages;
using CrossCut.Messages.OrderProcessingWorkflowModels.Warehouse;
using Dapr.Client;
using Dapr.Workflow;

namespace DaprWorkflowAspireDemo.Order.Api.Activities;

public class ReserveInventoryActivity : WorkflowActivity<InventoryItemCreateReservationRequest, InventoryItemReservationResult>
{
    private static readonly string StoreName = "statestore";
    private readonly DaprClient _client;
    private readonly ILogger _logger;

    public ReserveInventoryActivity(ILoggerFactory loggerFactory, DaprClient client)
    {
        _logger = loggerFactory.CreateLogger<ReserveInventoryActivity>();
        _client = client;
    }

    public override async Task<InventoryItemReservationResult> RunAsync(WorkflowActivityContext context, InventoryItemCreateReservationRequest req)
    {
        _logger.LogInformation(
            "Reserving inventory for order '{requestId}' of {quantity} {name}",
            req.RequestId,
            req.Quantity,
            req.ItemName);

        // Ensure that the store has items
        var result = await _client.InvokeMethodAsync<InventoryItemCreateReservationRequest, InventoryItemReservationResult>(
            HttpMethod.Post, DaprWarehouseConsts.DAPR_ID, "InventoryItemReservation", req);

        if (result.Success) return new InventoryItemReservationResult(true, result.OrderPayload);

        // Not enough items, or item does not exsists.
        return new InventoryItemReservationResult(false, result.OrderPayload);

        //var item = await _client.GetStateAsync<InventoryItem>(
        //    StoreName,
        //    req.ItemName.ToLowerInvariant());

        //// Catch for the case where the statestore isn't setup
        //if (item == null)
        //    // Not enough items.
        //    return new InventoryItemReservationResult(false, item);

        //_logger.LogInformation(
        //    "There are {quantity} {name} available for purchase",
        //    item.Quantity,
        //    item.Name);

        //// See if there're enough items to purchase
        //if (item.Quantity >= req.Quantity)
        //{
        //    // Simulate slow processing
        //    await Task.Delay(TimeSpan.FromSeconds(2));

        //    return new InventoryItemReservationResult(true, item);
        //}

        //// Not enough items.
        //return new InventoryItemReservationResult(false, item);
    }
}