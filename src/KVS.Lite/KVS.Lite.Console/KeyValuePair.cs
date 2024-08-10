namespace KVS.Lite.Console;

public class KeyValueStore
{
    private readonly Dictionary<string, object> keyValuePairs;
    private readonly Dictionary<string, DateTime> expirationTime;

    public KeyValueStore()
    {
        keyValuePairs = new Dictionary<string, Object>();
        expirationTime = new Dictionary<string, DateTime>();
    }

    public void Set(string key, object value, int ttl)
    {
        keyValuePairs[key] = value;
        expirationTime[key] = DateTime.UtcNow.Add(TimeSpan.FromSeconds(ttl));
    }

    public object Get(string key)
    {
        if (expirationTime.ContainsKey(key) && expirationTime[key] < DateTime.UtcNow)
        {
            keyValuePairs.Remove(key);
            expirationTime.Remove(key);
            System.Console.WriteLine($"Removing key {key} as it reached the ttl");
            return null;
        }
        return keyValuePairs[key];
        
    }
}