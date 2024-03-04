using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<DaprWorkflowAspireDemo_Order_Api>("order-service").WithDaprSidecar("order");
builder.AddProject<DaprWorkflowAspireDemo_Payment_Api>("delivery-payment").WithDaprSidecar("payment");
builder.AddProject<DaprWorkflowAspireDemo_Kitchen_Api>("kitchen-service").WithDaprSidecar("kitchen");
builder.AddProject<DaprWorkflowAspireDemo_Delivery_Api>("delivery-service").WithDaprSidecar("delivery");
builder.AddProject<DaprWorkflowAspireDemo_IntegrationTests_Api>("integration-tests").WithDaprSidecar("tests");


builder.Build().Run();