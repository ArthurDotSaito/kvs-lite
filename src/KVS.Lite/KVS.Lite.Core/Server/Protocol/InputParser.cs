using Newtonsoft.Json;
namespace KVS.Lite.Console.Server.Protocol;

public class InputParser
{
    public class Command()
    {
        public string Operation { get; set; }
        public string Key { get; set; }
        public Object Value { get; set; }
        public string Ttl { get; set; }
    }

    public Command CommandParser(string input)
    {
        var command = new Command();
        try
        {
            command = JsonConvert.DeserializeObject<Command>(input);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
        }

        return command;
    }
}