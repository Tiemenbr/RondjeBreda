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

    private async Task dropTables()
    {
        await _connection.DropTableAsync<RouteComponent>();
        await _connection.DropTableAsync<Route>();
        await _connection.DropTableAsync<Location>();
        await _connection.DropTableAsync<Description>();
    }

    #region Table Setups
    private async Task SetupLocationTable()
    {
        try
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
        catch (Exception)
        {
            Debug.WriteLine("Could not create Location table");
        }
    }
    private async Task SetupRouteComponentTable()
    {
        try
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
        catch (Exception)
        {
            Debug.WriteLine("Could not create RouteComponent table");
        }
    }
    private async Task SetupDescriptionTable()
    {
        try
        {
            await _connection.ExecuteAsync(
            @"CREATE TABLE IF NOT EXISTS Description (
                DescriptionNL TEXT NOT NULL,
                DescriptionEN TEXT,
                PRIMARY KEY (DescriptionNL)
            );");
        }
        catch (Exception)
        {
            Debug.WriteLine("Could not create Description table");
        }
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
        try
        {
            return await _connection.Table<Route>().ToArrayAsync();
        } catch (Exception)
        {
            Debug.WriteLine("Could not get routes");
            return new Route[] { };
        }
    }    
    
    public async Task<Domain.Models.DatabaseModels.Location[]> GetLocationTableAsync()
    {
        try
        {
            return await _connection.Table<Domain.Models.DatabaseModels.Location>().ToArrayAsync();
        } catch (Exception)
        {
            Debug.WriteLine("Could not get locations");
            return new Domain.Models.DatabaseModels.Location[] { };
        }
    }    
    
    public async Task<Description[]> GetDescriptionTableAsync()
    {
        try
        {
            return await _connection.Table<Description>().ToArrayAsync();
        }
        catch (Exception)
        {
            Debug.WriteLine("Could not get Descriptions");
            return new Description[] { };
        }
    }

    public async Task<RouteComponent[]> GetRouteComponentTableAsync()
    {
        try
        {
            return await _connection.Table<RouteComponent>().ToArrayAsync();
        }
        catch (Exception)
        {
            Debug.WriteLine("Could not get route components");
            return new RouteComponent[] { };
        }
    }

    public async Task<RouteComponent[]> GetRouteComponentFromRouteAsync(string routeName)
    {
        try
        {
            return await _connection.Table<RouteComponent>()
            .Where(routeComponent => routeComponent.RouteName == routeName)
            .ToArrayAsync();
        }
        catch (Exception)
        {
            Debug.WriteLine($"Could not get Route components from route {routeName}");
            return new RouteComponent[] { };
        }
        
    }
    
    public async Task<RouteComponent[]> GetVisitedRouteComponentsFromRouteAsync(string routeName)
    {
        try
        {
            return await _connection.Table<RouteComponent>()
            .Where(routeComponent => routeComponent.RouteName == routeName
            && routeComponent.Visited).ToArrayAsync();
        } catch (Exception)
        {
            Debug.WriteLine($"Could not get route component with name {routeName}");
            return new RouteComponent[] { };
        }

    }
    #endregion

    #region GetDatabaseItemMethods
    public async Task<Domain.Models.DatabaseModels.Route> GetRouteAsync(string routeName)
    {
        try
        {
        return await _connection.Table<Domain.Models.DatabaseModels.Route>()
            .Where(route => route.Name == routeName)
            .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            Debug.WriteLine($"Could not get route {routeName}");
            return new Route() { };
        }
    }    
    
    public async Task<Domain.Models.DatabaseModels.Location> GetLocationAsync(double longitude, double latitude)
    {
        try
        {
        return await _connection.Table<Domain.Models.DatabaseModels.Location>()
            .Where(location => location.Longitude == longitude && location.Latitude == latitude)
            .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            Debug.WriteLine($"Could not get location with long {longitude} en lat {latitude}");
            return null;
        }
    }    
    
    public async Task<Description> GetDescriptionAsync(string descriptionNL)
    {
        try
        {
            return await _connection.Table<Description>()
            .Where(description => description.DescriptionNL == descriptionNL)
            .FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            Debug.WriteLine($"Could not get Description with key {descriptionNL}");
            return new Description() { };
        }
        
    }

    public async Task<RouteComponent> GetRouteComponentAsync(string routeName, double longitude, double latitude)
    {
        try
        {
            return await _connection.Table<RouteComponent>()
            .Where(routeComponent =>
            routeComponent.RouteName == routeName
            && routeComponent.LocationLongitude == longitude
            && routeComponent.LocationLatitude == latitude
            ).FirstOrDefaultAsync();
        } catch (Exception)
        {
            Debug.WriteLine($"Could not get route component with " +
                $"\nName {routeName}" +
                $"\nLong {longitude}" +
                $"\nLat {latitude}");
            return new RouteComponent() { };
        }
        
    }
    #endregion

    #region UpdateDatabaseItemMethods
    public async Task UpdateRoute(string name, bool newActiveState)
    {
        try
        {
        await _connection.UpdateAsync(new Domain.Models.DatabaseModels.Route { Name = name, Active = newActiveState });
        } catch (Exception)
        {
            Debug.WriteLine($"Could not update route {name}");
        }
    }

    public async Task UpdateRouteComponent(string routeName, double longitude, double latitude, bool newIsVisited)
    {
        try
        {
        RouteComponent routeComponent = await GetRouteComponentAsync(routeName, longitude, latitude);
        routeComponent.Visited = newIsVisited;

        string query = @"UPDATE RouteComponent 
                         SET Visited = ?
                         WHERE RouteName = ? AND LocationLongitude = ? AND LocationLatitude = ?";
        await _connection.ExecuteAsync(query, newIsVisited, routeName, longitude, latitude);

        } catch (Exception)
        {
            Debug.WriteLine("Could not update route component with" +
                $"\nName: {routeName}" +
                $"\nLong: {longitude}" +
                $"\nLat: {latitude}");
        }
    }
    #endregion
}