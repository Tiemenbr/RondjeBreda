namespace RondjeBreda.Domain.Interfaces;

/// <summary>
/// Interface for the database connection
/// </summary>
public interface IDatabase
{
    Task Init();

    private async Task addTable(IDatabaseTable databaseTable);

    public async Task<Route[]> GetRouteTableAsync();

    public async Task<Route[]> GetRouteTableAsync();

    public async Task<Route[]> GetRouteTableAsync();
}