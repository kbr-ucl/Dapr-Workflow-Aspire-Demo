using CrossCut.Messages;
using Dapr.Client;
using Dapr.Workflow;
using DaprWorkflowAspireDemo.Order.Api.Workflows;

namespace DaprWorkflowAspireDemo.Order.Api.Activities;

internal class UpdateInventoryActivity : WorkflowActivity<PaymentRequest, object>
{
    private static readonly string StoreName = "statestore";
    private readonly DaprClient _client;
    private readonly ILogger _logger;

    public UpdateInventoryActivity(ILoggerFactory loggerFactory, DaprClient client)
    {
        _logger = loggerFactory.CreateLogger<UpdateInventoryActivity>();
        _client = client;
    }

    public override async Task<object> RunAsync(WorkflowActivityContext context, PaymentRequest req)
    {
        //_logger.LogInformation(
        //    "Checking inventory for order '{requestId}' for {amount} {name}",
        //    req.RequestId,
        //    req.Amount,
        //    req.ItemName);

        //// Simulate slow processing
        //await Task.Delay(TimeSpan.FromSeconds(5));

        //// Determine if there are enough Items for purchase
        //var item = await _client.GetStateAsync<InventoryItem>(
        //    StoreName,
        //    req.ItemName.ToLowerInvariant());
        //var newQuantity = item.Quantity - req.Amount;
        //if (newQuantity < 0)
        //{
        //    _logger.LogInformation(
        //        "Payment for request ID '{requestId}' could not be processed. Insufficient inventory.",
        //        req.RequestId);
        //    throw new InvalidOperationException(
        //        $"Not enough '{req.ItemName}' inventory! Requested {req.Amount} but only {item.Quantity} available.");
        //}

        //// Update the statestore with the new amount of the item
        //await _client.SaveStateAsync(
        //    StoreName,
        //    req.ItemName.ToLowerInvariant(),
        //    new InventoryItem(req.ItemName, item.PerItemCost, newQuantity));

        //_logger.LogInformation(
        //    "There are now {quantity} {name} left in stock",
        //    newQuantity,
        //    item.Name);

        return null;
    }
}