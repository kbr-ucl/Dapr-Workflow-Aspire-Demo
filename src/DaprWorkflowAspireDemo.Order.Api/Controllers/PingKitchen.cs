using CrossCut.Messages.Ping;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace DaprWorkflowAspireDemo.Order.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingKitchen : ControllerBase
{
    private readonly DaprClient _daprClient;

    public PingKitchen(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpGet("/ping-kitchen")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var command = new PingCommand();
            var response = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("kitchen", "ping", command,
                cancellationToken);
            Console.WriteLine($"Returned: {response.Message}");
            return Ok($"Kitchen is up and running: {response.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Kitchen is down: {e.Message}");
        }
    }
}