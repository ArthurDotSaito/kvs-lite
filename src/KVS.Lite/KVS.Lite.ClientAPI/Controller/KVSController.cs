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
    public IActionResult Set([FromQuery] string message)
    {
            
        var res = _client.Set(message);
            
        return Ok(res);
    }

}