using RondjeBreda.Domain.Interfaces;

namespace RondjeBreda;

public class KeyReaderMaps : IKeyReaderMaps
{
    public async Task<string?> GetApiKeyAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("MapsKey.txt");
        using var reader = new StreamReader(stream);

        var contents = reader.ReadToEnd();
        return contents;
    }
}