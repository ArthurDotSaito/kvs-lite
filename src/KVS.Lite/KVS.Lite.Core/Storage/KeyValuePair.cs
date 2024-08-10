namespace KVS.Lite.Console.Storage;

public class KeyValueStore
{
    private readonly Dictionary<string, object> keyValuePairs;
    private readonly Dictionary<string, DateTime> expirationTime;
    private readonly TimeSpan defaultTtl = TimeSpan.MaxValue;

    public KeyValueStore()
    {
        keyValuePairs = new Dictionary<string, Object>();
        expirationTime = new Dictionary<string, DateTime>();
    }

    public void Set(string key, object value, int ttl = -1)
    {
        keyValuePairs[key] = value;
        var dateTime = (ttl == -1) ? DateTime.MaxValue : DateTime.UtcNow.Add(TimeSpan.FromSeconds(ttl));
        expirationTime[key] = dateTime;
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