using System.Text;
using CrossCut.Messages.Ping;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace DaprWorkflowAspireDemo.IntegrationTests.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingServices : ControllerBase
{
    private readonly DaprClient _daprClient;

    public PingServices(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    [HttpGet("/ping-order")]
    public async Task<IActionResult> PingOrdrer(CancellationToken cancellationToken)
    {
        try
        {
            var command = new PingCommand();
            var response = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("order", "ping", command,
                cancellationToken);
            Console.WriteLine($"Returned: {response.Message}");
            return Ok($"Order is up and running: {response.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Kitchen is down: {e.Message}");
        }
    }

    [HttpGet("/ping-payment")]
    public async Task<IActionResult> PingPayment(CancellationToken cancellationToken)
    {
        try
        {
            var command = new PingCommand();
            var response = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("payment", "ping", command,
                cancellationToken);
            Console.WriteLine($"Returned: {response.Message}");
            return Ok($"Payment is up and running: {response.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Kitchen is down: {e.Message}");
        }
    }

    [HttpGet("/ping-kitchen")]
    public async Task<IActionResult> PingKitchen(CancellationToken cancellationToken)
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


    [HttpGet("/ping-delivery")]
    public async Task<IActionResult> PingDelivery(CancellationToken cancellationToken)
    {
        try
        {
            var command = new PingCommand();
            var response = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("delivery", "ping", command,
                cancellationToken);
            Console.WriteLine($"Returned: {response.Message}");
            return Ok($"Delivery is up and running: {response.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Kitchen is down: {e.Message}");
        }
    }

    [HttpGet("/ping-all")]
    public async Task<IActionResult> PingAll(CancellationToken cancellationToken)
    {
        try
        {
            var responses = new List<string>();
            var command = new PingCommand();
            var responseOrder = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("order", "ping", command,
                cancellationToken);
            responses.Add($"Order is up and running: {responseOrder.Message}");

            var responsePayment = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("payment", "ping",
                command,
                cancellationToken);
            responses.Add($"Payment is up and running: {responsePayment.Message}");

            var responseKitchen = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("kitchen", "ping",
                command,
                cancellationToken);
            responses.Add($"Kitchen is up and running: {responseKitchen.Message}");

            var responseDelivery = await _daprClient.InvokeMethodAsync<PingCommand, PingResponse>("delivery", "ping",
                command,
                cancellationToken);
            responses.Add($"Delivery is up and running: {responseDelivery.Message}");


            var sb = new StringBuilder();
            foreach (var response in responses)
            {
                sb.AppendLine(response);
                Console.WriteLine($"Returned: {response}");
            }

            return Ok($"{sb}");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Kitchen is down: {e.Message}");
        }
    }
}