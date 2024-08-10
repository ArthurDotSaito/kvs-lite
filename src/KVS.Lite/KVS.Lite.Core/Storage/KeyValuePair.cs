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

    public StatusProtocol Set(string key, object value, double ttl = -1)
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

    public StatusProtocol Get(string key)
    {
        var status = new StatusProtocol();
        
        if (string.IsNullOrEmpty(key))
        {
            status.Status = StatusCode.Error;
            status.Message = "Key is null or empty";
            return status;
        }

        if (!KeyExists(key))
        {
            status.Status = StatusCode.Error;
            status.Message = "Key does not exist!";
            return status;
        }

        if (expirationTime[key] >= DateTime.UtcNow)
        {
            status.Status = StatusCode.Sucess;
            status.Message = StatusCode.Sucess;
            status.Value = keyValuePairs[key];
            return status;
        }
        
        CheckAndRemoveExpiredKey(key);
        status.Status = StatusCode.Error;
        status.Message = "Key does not exist!";
        return status;
    }

    public StatusProtocol Delete(string key)
    {
        var status = new StatusProtocol();
        if (string.IsNullOrEmpty(key))
        {
            status.Status = StatusCode.Error;
            status.Message = "Key is null or empty";
            return status;
        }
        
        if (!KeyExists(key))
        {
            status.Status = StatusCode.Error;
            status.Message = "Key does not exist!";
            return status;
        }
        
        CheckAndRemoveExpiredKey(key);

        if (keyValuePairs.Remove(key))
        {
            expirationTime.Remove(key);
        }
        
        status.Status = StatusCode.Sucess; 
        status.Message = "Key removed successfully"; 
        return status;
    }
    
    public StatusProtocol Update(string key, object value)
    {
        var status = new StatusProtocol();
        if (string.IsNullOrEmpty(key))
        {
            status.Status = StatusCode.Error;
            status.Message = "Key is null or empty";
            return status;
        }
        
        if (!KeyExists(key))
        {
            status.Status = StatusCode.Error;
            status.Message = "Key does not exist!";
            return status;
        }

        if (CheckExpiredKey(key))
        {
            CheckAndRemoveExpiredKey(key);
            status.Status = StatusCode.Error;
            status.Message = "Key does not exist";
            return status;
        }
        
        
        keyValuePairs[key] = value;
        status.Message = "Updated Successfully";
        return status;
    }

    private bool KeyExists(string key)
    {
        return keyValuePairs.ContainsKey(key) && expirationTime.ContainsKey(key);
    }

    private void CheckAndRemoveExpiredKey(string key)
    {
        if (CheckExpiredKey(key))
        {
            keyValuePairs.Remove(key);
            expirationTime.Remove(key);
        }
    }

    private bool CheckExpiredKey(string key)
    {
        return expirationTime[key] < DateTime.UtcNow;
    }
}