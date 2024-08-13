using System.Net;
using System.Net.Sockets;
using System.Text;
using KVS.Lite.Console.Model;
using KVS.Lite.Console.Server.Protocol;
using KVS.Lite.Console.Storage;

namespace KVS.Lite.Console.Server;

public class Server
{
    private const int Port = 6355;
    private readonly TcpListener _listener;
    private readonly KeyValueStore _keyValueStore;

    public Server(KeyValueStore keyValueStore)
    {
        this._keyValueStore = keyValueStore;
        _listener = new TcpListener(IPAddress.Any, Port);
    }

    public void Start()
    {
        _listener.Start();
        System.Console.WriteLine($"Server started. Listening on port {Port}");
        
        
        while (true)
        {
            var client = _listener.AcceptTcpClient();
            ClientHandler(client);
        }
    }

    private void ClientHandler(TcpClient client)
    {
        try
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            while (client.Connected)
            {
                try
                {
                    if (!stream.DataAvailable) break;

                    var buffer = new byte[1024];
                    var messageBuilder = new StringBuilder();

                    while (true)
                    {
                        var bytesRead = stream.Read(buffer, 0, buffer.Length);
                        messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                        if (stream.DataAvailable) continue;

                        break;
                    }

                    var dataReceived = Encoding.UTF8.GetBytes(messageBuilder.ToString());
                    var stringReceived = Encoding.UTF8.GetString(dataReceived);
                    System.Console.WriteLine(stringReceived);

                    var encodedResponse = Encoding.UTF8.GetBytes(stringReceived);
                    stream.Write(encodedResponse, 0, encodedResponse.Length);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
        }
        
        client.Close();
    }

    private StatusProtocol ProcessRequest(string request)
    {
        var parser = new InputParser();
        var command = parser.CommandParser(request);

        if (command.Operation == "SET")
        {
            double ttl = -1;
            if (!double.TryParse(command.Ttl, out ttl)) ttl = -1;

            return this._keyValueStore.Set(command.Key, command.Value.ToString(), ttl);
        }
        else if (command.Operation == "GET")
        {
            return this._keyValueStore.Get(command.Key);
        }
        else if (command.Operation == "DELETE")
        {
            return this._keyValueStore.Delete(command.Key);
        }
        else if (command.Operation == "UPDATE")
        {
            return this._keyValueStore.Update(command.Key, command.Value.ToString());
        }

        return new StatusProtocol() { Status = StatusCode.Error, Message = "A Unexpected error ocurred." };
    }
    
    
}