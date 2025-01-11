using Microsoft.Maui.Storage;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;
using SQLite;
using System;
using System.Diagnostics;
using System.Text.Json;

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


    // private SQLiteAsyncConnection databaseConnection;
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
        await _connection.CreateTableAsync<Route>(); // Isn't complicated like others
        await SetupRouteComponentTable();

        CompleteRouteContent content = await ConvertRouteDataToObject();

        if (content != null) 
        {

            Route TempRoute = new Route {
                Name = "HistorischeKilometer",
                Active = false
            };

            await _connection.InsertAsync(TempRoute);

            foreach (RouteLocation location in content.HistorischeKilometerRoute) {
                Description TempDescription = new Description {
                    DescriptionNL = location.CommentDutch,
                    DescriptionEN = location.CommentEnglish
                };

                Location Templocation = new Domain.Models.DatabaseModels.Location {
                    Longitude = location.Longitude,
                    Latitude = location.Latitude,
                    Description = TempDescription.DescriptionNL,
                    ImagePath = $"location_{location.PhotoNr}.jpg",
                    Name = location.LocationName
                };

                TempDescription.Location = Templocation;

                await _connection.InsertAsync(Templocation);
                await _connection.InsertAsync(TempDescription);
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
                FOREIGN KEY (Description) REFERENCES Description(DescriptionNL)
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
                LocationLongitude REAL,
                LocationLatitude REAL,
                PRIMARY KEY (DescriptionNL),
                FOREIGN KEY (LocationLongitude, LocationLatitude) REFERENCES Location(Longitude, Latitude)
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

    private async Task addTable(IDatabaseTable databaseTable) 
    {
        await _connection.InsertAsync(databaseTable);
    }

    public void GetDatabaseTableAsync() 
    {
        throw new NotImplementedException();
    }

    public void GetDatabaseItemAsync() 
    {
        throw new NotImplementedException();
    }

    public void UpdateDatabaseItemAsync() 
    {
        throw new NotImplementedException();
    }
}