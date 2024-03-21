using CrossCut.Messages;
using Dapr.Workflow;
using DaprWorkflowAspireDemo.Order.Api.Workflows;

namespace DaprWorkflowAspireDemo.Order.Api.Activities;

public class RequestApprovalActivity : WorkflowActivity<OrderPayload, object>
{
    private readonly ILogger _logger;

    public RequestApprovalActivity(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<RequestApprovalActivity>();
    }

    public override Task<object> RunAsync(WorkflowActivityContext context, OrderPayload input)
    {
        var orderId = context.InstanceId;
        _logger.LogInformation("Requesting approval for order {orderId}", orderId);

        return Task.FromResult<object>(null);
    }
}