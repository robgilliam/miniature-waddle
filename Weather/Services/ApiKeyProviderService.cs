namespace Weather.Services;

public abstract class ApiKeyProviderService : IApiKeyProviderService
{
    protected readonly IDictionary<object, string> _keys = new Dictionary<object, string>();

    public IEnumerable<object> GetServices()
    {
        return _keys.Keys;
    }

    public string? GetServiceApiKey(object serviceId)
    {
        string? key = null;

        _keys.TryGetValue(serviceId, out key);

        return key;
    }

    public bool TryGetServiceApiKey(object serviceId, out string? key)
    {
        return _keys.TryGetValue(serviceId, out key);
    }
}