using CrossCut.Messages;
using Dapr.Workflow;
using DaprWorkflowAspireDemo.Order.Api.Workflows;

namespace DaprWorkflowAspireDemo.Order.Api.Activities;

public class ProcessPaymentActivity : WorkflowActivity<PaymentRequest, object>
{
    private readonly ILogger logger;

    public ProcessPaymentActivity(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<ProcessPaymentActivity>();
    }

    public override async Task<object> RunAsync(WorkflowActivityContext context, PaymentRequest req)
    {
        logger.LogInformation(
            "Processing payment: {requestId} for {amount} {item} at ${currency}",
            req.RequestId,
            req.Amount,
            req.ItemName,
            req.Currency);

        // Simulate slow processing
        await Task.Delay(TimeSpan.FromSeconds(7));

        logger.LogInformation(
            "Payment for request ID '{requestId}' processed successfully",
            req.RequestId);

        return null;
    }
}