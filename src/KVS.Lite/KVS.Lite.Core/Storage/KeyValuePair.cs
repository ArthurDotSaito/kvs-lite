using KVS.Lite.Console.Model;

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

    public StatusProtocol Set(string key, object value, int ttl = -1)
    {
        var status = new StatusProtocol();
        try
        {
            if (string.IsNullOrEmpty(key))
            {
                status.Status = StatusCode.Error;
                status.Message = "Key is null or empty";
                return status;
            }

            if (KeyExists(key))
            {
                status.Status = StatusCode.Error;
                status.Message = "Key already exist!";
                return status;
            }

            keyValuePairs[key] = value;
            var dateTime = (ttl == -1) ? DateTime.MaxValue : DateTime.UtcNow.Add(TimeSpan.FromSeconds(ttl));
            expirationTime[key] = dateTime;
        }
        catch (Exception ex)
        {
            status.Status = StatusCode.Error;
            status.Message = ex.Message;
        }

        status.Message = "Successfully stored!";
        return status;
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

    private bool KeyExists(string key)
    {
        return keyValuePairs.ContainsKey(key) && expirationTime.ContainsKey(key);
    }
}