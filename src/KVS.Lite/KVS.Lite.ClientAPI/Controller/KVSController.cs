using Microsoft.AspNetCore.Mvc;

namespace KVS.Lite.ClientAPI.KVSController;


[Route("/api/[controller]")]
public class ClientController : Controller, IDisposable
{
    private readonly KVSClient.KVSClient _client;
    public ClientController()
    {
        _client = new KVSClient.KVSClient();
    }
    
    [HttpGet]
    [Route("set")]
    public IActionResult Set([FromQuery] string key, string value, string timeToLeave)
    {
            
        var res = _client.Set(key, value, timeToLeave);
            
        return Ok(res);
    }

}