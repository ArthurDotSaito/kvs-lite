using KVS.Lite.Console.Server;
using KVS.Lite.Console.Storage;

Console.WriteLine("Starting KVS Store");

var kvStore = new KeyValueStore();

var server = new Server(kvStore);

server.Start();