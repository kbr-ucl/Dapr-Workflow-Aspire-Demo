using Dapr.Workflow;
using DaprWorkflowAspireDemo.Order.Api.Activities;
using DaprWorkflowAspireDemo.Order.Api.Workflows;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
var jsonOpt = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
};

builder.Services.AddControllers().AddDapr(opt => opt.UseJsonSerializationOptions(jsonOpt));

builder.Services.AddDaprWorkflow(options =>
{
    // Note that it's also possible to register a lambda function as the workflow
    // or activity implementation instead of a class.
    options.RegisterWorkflow<OrderProcessingWorkflow>();

    // These are the activities that get invoked by the workflow(s).
    options.RegisterActivity<NotifyActivity>();
    options.RegisterActivity<ProcessPaymentActivity>();
    options.RegisterActivity<RequestApprovalActivity>();
    options.RegisterActivity<ReserveInventoryActivity>();
    options.RegisterActivity<UpdateInventoryActivity>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

// Dapr configurations
app.UseCloudEvents();
app.MapSubscribeHandler();

app.MapControllers();

app.Run();
