namespace Weather.Services;

public class ApiKeyFileProviderService : ApiKeyProviderService
{
    public ApiKeyFileProviderService(FileInfo file)
    {
        try
        {
            using (var stream = file.OpenText())
            {
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    var setting = line.Split(':');

                    _keys.Add(setting[0], setting[1]);
                }
            }
        }
        catch (Exception)
        {
            // TODO handle IO exception
        }
    }
}