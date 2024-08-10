using System.Net.Sockets;
using System.Text;

namespace KVS.Lite.Client;

public class Client
{
    private const int Port = 6355;
    private const string Host = "localhost";
    private TcpClient _client;
    private NetworkStream _stream;
    private StreamWriter _writer;
    private StreamReader _reader;

    public Client()
    {
        Console.WriteLine($"$KVLite Client initiated for {Host}:{Port}");
        _client = new TcpClient(Host, Port);
        _stream = _client.GetStream();
        _writer = new StreamWriter(_stream, Encoding.ASCII) { AutoFlush = true };
        _reader = new StreamReader(_stream, Encoding.ASCII);
    }

    public string Set(string message)
    {
        _writer.WriteLine(message);
        string response = _reader.ReadLine();
        Console.WriteLine(response);

        return response;
    }
}