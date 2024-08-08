

using KVS.Lite.Console;

Console.WriteLine("Starting KVS Store");

var kvStore = new KeyValueStore();

kvStore.Set("Test Key", "Test Value");

Console.WriteLine(kvStore.Get("Test Key"));