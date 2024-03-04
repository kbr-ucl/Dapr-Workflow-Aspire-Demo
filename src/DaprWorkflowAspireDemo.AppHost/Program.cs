var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DaprWorkflowAspireDemo_Order_Api>("daprworkflowaspiredemo.order.api");

builder.AddProject<Projects.DaprWorkflowAspireDemo_Kitchen_Api>("daprworkflowaspiredemo.kitchen.api");

builder.AddProject<Projects.DaprWorkflowAspireDemo_Delivery_Api>("daprworkflowaspiredemo.delivery.api");

builder.Build().Run();
