namespace KVS.Lite.Console;

public class KeyValueStore
{
    private readonly Dictionary<string, object> keyValuePairs;

    public KeyValueStore()
    {
        keyValuePairs = new Dictionary<string, Object>();
    }

    public void Set(string key, object value)
    {
        keyValuePairs[key] = value;
    }

    public object Get(string key)
    {
        return keyValuePairs[key];
    }
}