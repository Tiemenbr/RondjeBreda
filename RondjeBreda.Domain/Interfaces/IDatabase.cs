namespace RondjeBreda.Domain.Interfaces;

/// <summary>
/// Interface for the database connection
/// </summary>
public interface IDatabase
{
    Task Init();
    void GetDatabaseTableAsync();
    void GetDatabaseItemAsync();
    void UpdateDatabaseItemAsync();
}