using System.Net.Sockets;
using System.Text;

namespace KVS.Lite.KVSClient;

public class KVSClient
{
    private const int Port = 6355;
    private const string Host = "localhost";
    private TcpClient _client;
    private NetworkStream _stream;
    private StreamWriter _writer;
    private StreamReader _reader;

    public KVSClient()
    {
        Console.WriteLine($"$KVLite Client initiated for {Host}:{Port}");
        _client = new TcpClient(Host, Port);
        _stream = _client.GetStream();
        _writer = new StreamWriter(_stream, Encoding.ASCII) { AutoFlush = true };
        _reader = new StreamReader(_stream, Encoding.ASCII);
    }

    public string Set(string key, string value, string timeToLive)
    {
        string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\", \"value\": \"{value}\",\"ttl\": \"{timeToLive}\"}}";
        _writer.WriteLine(command);
        
        string response = _reader.ReadLine();
        Console.WriteLine(response);

        return response;
    }

    public string Get(string key)
    {
        string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\"}}";
        _writer.WriteLine(command);

        string response = _reader.ReadLine();
        Console.WriteLine(response);

        return response;
    }

    public string Update(string key)
    {
        string command = $"{{\"Operation\": \"SET\", \"key\": \"{key}\"}}";
        _writer.WriteLine(command);

        string response = _reader.ReadLine();
        Console.WriteLine(response);

        return response;
    }

    public void Dispose()
    {
        _writer.Dispose();
        _reader.Dispose();
        _stream.Dispose();
        _client.Close();
    }
}