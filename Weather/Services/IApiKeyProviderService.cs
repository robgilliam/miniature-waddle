namespace Weather.Services;

public interface IApiKeyProviderService
{
    // Returns a list of services for which the keys are known
    IEnumerable<object> GetServices();

    // Returns <code>null</code> if the key for the service is not known
    string? GetServiceApiKey(object serviceId);

    // Returns <code>false</code> if the key for the service is not known
    bool TryGetServiceApiKey(object serviceId, out string? key);
}