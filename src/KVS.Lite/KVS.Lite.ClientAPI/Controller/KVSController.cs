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
    
    [HttpGet]
    [Route("get")]
    public IActionResult Get([FromQuery] string key)
    {
            
        var res = _client.Get(key);
            
        return Ok(res);
    }
    
    [HttpGet]
    [Route("delete")]
    public IActionResult Delete([FromQuery] string key)
    {
            
        var res = _client.Delete(key);
            
        return Ok(res);
    }

}