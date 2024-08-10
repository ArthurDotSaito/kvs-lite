namespace KVS.Lite.Console.Model;

public class StatusProtocol
{
    public string Status { get; set; } = StatusCode.Sucess;
    public string Message { get; set; }
    public object Value { get; set; }
}