using Microsoft.Maui.Storage;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;
using SQLite;
using System.Diagnostics;
using System.Text.Json;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Route = RondjeBreda.Domain.Models.DatabaseModels.Route;

namespace RondjeBreda.Infrastructure.DatabaseImplementation;

/// <summary>
/// The implementation for the connection with te SQLite database
/// </summary>
public class SQLiteDatabase : IDatabase
{
    private const string DatabaseName = "RondjeBredaDatabase";
    private SQLiteAsyncConnection _connection;

    public SQLiteDatabase() {
    }

    public async Task Init() 
    {
        if (_connection != null) 
        {
            return;
        }

        string path = Path.Combine(FileSystem.AppDataDirectory, DatabaseName);
        Debug.WriteLine(path);
        if (!File.Exists(path))
        {
            try
            {
                File.Create($"{path}.db3").Close();
            } catch (UnauthorizedAccessException) 
            {
                Debug.WriteLine("App doesn't have permissions to create file");
            } catch (Exception)
            {
                Debug.WriteLine("Could not make file!");
            }
            
        }
        
        _connection = new SQLiteAsyncConnection(path);
        await _connection.DropTableAsync<RouteComponent>();
        await _connection.DropTableAsync<Route>();
        await _connection.DropTableAsync<Location>();
        await _connection.DropTableAsync<Description>();


        await SetupDescriptionTable();
        await SetupLocationTable();
        await _connection.CreateTableAsync<Domain.Models.DatabaseModels.Route>(); // Isn't complicated like others
        await SetupRouteComponentTable();

        await _connection.ExecuteAsync("PRAGMA foreign_keys = ON;");

        Domain.Models.DatabaseModels.Route historischeKilometer = await _connection.Table<Domain.Models.DatabaseModels.Route>().Where(route => route.Name == "HistorischeKilometer").FirstOrDefaultAsync();

        if (historischeKilometer != null)
            return;

        CompleteRouteContent content = await ConvertRouteDataToObject();

        if (content != null) 
        {

            Domain.Models.DatabaseModels.Route TempRoute = new Domain.Models.DatabaseModels.Route
            {
                Name = "Historische Kilometer",
                Active = false
            };

            await _connection.InsertAsync(TempRoute);

            int noDescriptionCounter = 0;

            foreach (RouteLocation location in content.HistorischeKilometerRoute) {
                noDescriptionCounter++;
                Debug.WriteLine($"Longitude: {location.Longitude} & Latitude: {location.Latitude}");
                Description TempDescription = new Description {
                    DescriptionNL = location.CommentDutch,
                    DescriptionEN = location.CommentEnglish
                };

                if (TempDescription.DescriptionNL == null)
                {
                    TempDescription.DescriptionNL = $"NoDescription{noDescriptionCounter}";
                }

                await _connection.InsertAsync(TempDescription);

                Domain.Models.DatabaseModels.Location Templocation = new Domain.Models.DatabaseModels.Location {
                    Longitude = location.Longitude,
                    Latitude = location.Latitude,
                    Description = TempDescription.DescriptionNL,
                    ImagePath = $"location_{location.PhotoNr}.jpg",
                    Name = location.LocationName
                };

                await _connection.InsertAsync(Templocation);

                await _connection.InsertAsync(new RouteComponent {
                    RouteName = TempRoute.Name,
                    LocationLongitude = Templocation.Longitude,
                    LocationLatitude = Templocation.Latitude,
                    Note = location.CommentDutch,
                    RouteOrderNumber = location.routeNr,
                    Visited = false
                });

            }
        }
    }


    #region Table Setups
    private async Task SetupLocationTable()
    {
        await _connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS Location (
                Longitude REAL NOT NULL,
                Latitude REAL NOT NULL,
                Description TEXT,
                ImagePath TEXT,
                Name TEXT,
                PRIMARY KEY (Longitude, Latitude),
                FOREIGN KEY (Description) REFERENCES Description(DescriptionNL) ON DELETE CASCADE ON UPDATE CASCADE
            );");
    }
    private async Task SetupRouteComponentTable()
    {
        await _connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS RouteComponent (
                RouteName TEXT NOT NULL,
                LocationLongitude REAL NOT NULL,
                LocationLatitude REAL NOT NULL,
                Note TEXT,
                Visited BOOLEAN,
                RouteOrderNumber INTEGER,
                PRIMARY KEY (RouteName, LocationLongitude, LocationLatitude),
                FOREIGN KEY (RouteName) REFERENCES Route(Name),
                FOREIGN KEY (LocationLongitude, LocationLatitude) REFERENCES Location(Longitude, Latitude)
            );");
    }
    private async Task SetupDescriptionTable()
    {
        await _connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS Description (
                DescriptionNL TEXT NOT NULL,
                DescriptionEN TEXT,
                PRIMARY KEY (DescriptionNL)
            );");
    }

    #endregion
    private async Task<CompleteRouteContent> ConvertRouteDataToObject() 
    {

        using var stream = await FileSystem.OpenAppPackageFileAsync("Configuration.JSON");
        using var reader = new StreamReader(stream);

        string json = await reader.ReadToEndAsync();

        return JsonSerializer.Deserialize<CompleteRouteContent>(json);
    }

    #region GetTableMethods
    public async Task<Domain.Models.DatabaseModels.Route[]> GetRouteTableAsync()
    {
        return await _connection.Table<Route>().ToArrayAsync();
    }    
    
    public async Task<Domain.Models.DatabaseModels.Location[]> GetLocationTableAsync()
    {
        return await _connection.Table<Domain.Models.DatabaseModels.Location>().ToArrayAsync();
    }    
    
    public async Task<Description[]> GetDescriptionTableAsync()
    {
        return await _connection.Table<Description>().ToArrayAsync();
    }

    public async Task<RouteComponent[]> GetRouteComponentTableAsync()
    {
        return await _connection.Table<RouteComponent>().ToArrayAsync();
    }

    public async Task<RouteComponent[]> GetRouteComponentFromRouteAsync(string routeName)
    {
        return await _connection.Table<RouteComponent>()
            .Where(routeComponent => routeComponent.RouteName == routeName)
            .ToArrayAsync();
    }
    
    public async Task<RouteComponent[]> GetVisitedRouteComponentsFromRouteAsync(string routeName)
    {
        return await _connection.Table<RouteComponent>()
            .Where(routeComponent => routeComponent.RouteName == routeName
            && routeComponent.Visited).ToArrayAsync();
    }
    #endregion

    #region GetDatabaseItemMethods
    public async Task<Domain.Models.DatabaseModels.Route> GetRouteAsync(string routeName)
    {
        return await _connection.Table<Domain.Models.DatabaseModels.Route>()
            .Where(route => route.Name == routeName)
            .FirstOrDefaultAsync();
    }    
    
    public async Task<Domain.Models.DatabaseModels.Location> GetLocationAsync(double longitude, double latitude)
    {
        return await _connection.Table<Domain.Models.DatabaseModels.Location>()
            .Where(location => location.Longitude == longitude && location.Latitude == latitude)
            .FirstOrDefaultAsync();
    }    
    
    public async Task<Description> GetDescriptionAsync(string descriptionNL)
    {
        return await _connection.Table<Description>()
            .Where(description => description.DescriptionNL == descriptionNL)
            .FirstOrDefaultAsync();
    }

    public async Task<RouteComponent> GetRouteComponentAsync(string routeName, double longitude, double latitude)
    {
        return await _connection.Table<RouteComponent>()
            .Where(routeComponent => 
            routeComponent.RouteName == routeName 
            && routeComponent.LocationLongitude == longitude 
            && routeComponent.LocationLatitude == latitude)
            .FirstOrDefaultAsync();
    }
    #endregion

    #region UpdateDatabaseItemMethods
    public async Task UpdateRoute(string name, bool newActiveState)
    {
        await _connection.UpdateAsync(new Domain.Models.DatabaseModels.Route { Name = name, Active = newActiveState });
    }

    public async Task UpdateRouteComponent(string routeName, double longitude, double latitude, bool newIsVisited)
    {
        RouteComponent routeComponent = await GetRouteComponentAsync(routeName, longitude, latitude);
        routeComponent.Visited = newIsVisited;

        string query = @"UPDATE RouteComponent 
                         SET Visited = ?
                         WHERE RouteName = ? AND LocationLongitude = ? AND LocationLatitude = ?";
        await _connection.ExecuteAsync(query, newIsVisited, routeName, longitude, latitude);

    }
    #endregion
}