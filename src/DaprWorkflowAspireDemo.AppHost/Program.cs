using System.Collections.Immutable;
using Aspire.Hosting.Dapr;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// https://devblogs.microsoft.com/dotnet/announcing-dotnet-aspire-preview-2/

//var stateStore = builder.AddDaprComponent(
//    "statestore",
//    "state.redis",
//    new DaprComponentOptions { LocalPath = "..\\components" });

//var pubSub = builder.AddDaprComponent(
//    "pubsub",
//    "pubsub.rabbitmq",
//    new DaprComponentOptions { LocalPath = "..\\components" });

//builder.AddProject<DaprWorkflowAspireDemo_Order_Api>("order-service")
//    .WithDaprSidecar("order")
//    .WithReference(stateStore)
//    .WithReference(pubSub );

builder.AddProject<DaprWorkflowAspireDemo_Order_Api>("order-service").WithDaprSidecar(
    new DaprSidecarOptions
    {
        AppId = "order",
        ResourcesPaths = ImmutableHashSet.Create("..\\components")
    }
);

builder.AddProject<DaprWorkflowAspireDemo_Warehouse_Api>("warehouse-service").WithDaprSidecar(
    new DaprSidecarOptions
    {
        AppId = "warehouse",
        ResourcesPaths = ImmutableHashSet.Create("..\\components")
    }
);


//builder.AddProject<DaprWorkflowAspireDemo_Order_Api>("order-service").WithDaprSidecar("order");
builder.AddProject<DaprWorkflowAspireDemo_Payment_Api>("delivery-payment").WithDaprSidecar("payment");
//builder.AddProject<DaprWorkflowAspireDemo_Warehouse_Api>("warehouse-service").WithDaprSidecar("warehouse");
builder.AddProject<DaprWorkflowAspireDemo_Delivery_Api>("delivery-service").WithDaprSidecar("delivery");
builder.AddProject<DaprWorkflowAspireDemo_IntegrationTests_Api>("integration-tests").WithDaprSidecar("tests");


builder.Build().Run();