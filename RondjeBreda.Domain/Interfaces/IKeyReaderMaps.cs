namespace RondjeBreda.Domain.Interfaces;

public interface IKeyReaderMaps
{
    Task<string?> GetApiKeyAsync();
}