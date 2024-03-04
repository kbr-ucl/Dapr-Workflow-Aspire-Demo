using CrossCut.Messages.Ping;
using Microsoft.AspNetCore.Mvc;

namespace DaprWorkflowAspireDemo.Kitchen.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PingHandlerController : ControllerBase
{
    [HttpPost("/ping")]
    public IActionResult Post(PingCommand command)
    {
        Console.WriteLine("Ping received");
        return Ok(new PingResponse { Message = "Kitchen is up and running" });
    }
}