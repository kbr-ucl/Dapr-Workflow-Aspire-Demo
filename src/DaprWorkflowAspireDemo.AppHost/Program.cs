var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DaprWorkflowAspireDemo_Order_Api>("order-service").WithDaprSidecar("order");

builder.AddProject<Projects.DaprWorkflowAspireDemo_Kitchen_Api>("kitchen-service").WithDaprSidecar("kitchen");

builder.AddProject<Projects.DaprWorkflowAspireDemo_Delivery_Api>("delivery-service").WithDaprSidecar("delivery");

builder.Build().Run();
