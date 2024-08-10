using KVS.Lite.Console.Storage;

Console.WriteLine("Starting KVS Store");

var kvStore = new KeyValueStore();

kvStore.Set("Test Key", "Test Value", 5);

Console.WriteLine(kvStore.Get("Test Key"));

Thread.Sleep(6000);
Console.WriteLine(kvStore.Get("Test Key"));