using GameServerManager.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/servers")]
public class ControllerHeartBeat : ControllerBase
{
    private static readonly Dictionary<string, HeartBeatStatus> Servers = new();

    [HttpPost("heartbeat")]
    public IActionResult Heartbeat([FromBody] HeartBeatStatus input)
    {
        input.LastSeen = DateTime.UtcNow;
        Servers[input.Id] = input;
        return Ok();
    }

    [HttpGet("list")]
    public IActionResult List()
    {
        var now = DateTime.UtcNow;
        var result = Servers.Values.Select(s => new
        {
            s.Id,
            s.Name,
            s.IP,
            s.Port,
            Online = (now - s.LastSeen).TotalSeconds < 5
        });

        return Ok(result);
    }
}