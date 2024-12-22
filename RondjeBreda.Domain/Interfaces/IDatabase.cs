namespace RondjeBreda.Domain.Interfaces;

/// <summary>
/// Interface for the database connection
/// </summary>
public interface IDatabase
{
    void init();
    void GetDatabaseTableAsync();
    void GetDatabaseItemAsync();
    void UpdateDatabaseItemAsync();
}