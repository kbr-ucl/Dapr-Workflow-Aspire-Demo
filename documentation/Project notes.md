

dotnet workload update
dotnet workload install aspire

dotnet workload list



I hosting:

PackageReference Include="Aspire.Hosting.Dapr" Version="8.0.0-preview.3.24105.21"



DaprWorkflowAspireDemo.Order.Api



Add Dapr

PackageReference Include="Dapr.AspNetCore" Version="1.12.0"

// Dapr configurations
app.UseCloudEvents();

app.MapSubscribeHandler();



https://learn.microsoft.com/en-us/dotnet/aspire/whats-new/preview-3#model-dapr-sidecar-as-an-aspire-resource

https://learn.microsoft.com/en-us/dotnet/api/aspire.hosting.dapr.daprsidecaroptions?view=dotnet-aspire-8.0

Sluk https

I hosting

https://github.com/dotnet/aspire-samples/blob/main/samples/AspireWithDapr/AspireWithDapr.Web/WeatherApiClient.cs

________________________________

