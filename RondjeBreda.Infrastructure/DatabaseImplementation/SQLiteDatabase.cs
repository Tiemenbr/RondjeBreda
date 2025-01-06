using Microsoft.Maui.Storage;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;
using SQLite;
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
        init();
    }


    // private SQLiteAsyncConnection databaseConnection;
    public async void init() {
        if (_connection != null) {
            return;
        }

        _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DatabaseName));
        _connection.CreateTableAsync<Description>();
        _connection.CreateTableAsync<Location>();
        _connection.CreateTableAsync<Route>();
        _connection.CreateTableAsync<RouteComponent>();

        CompleteRouteContent content = ConvertRouteDataToObject().Result;

        if (content != null) {

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

                Location Templocation = new Location {
                    Longitude = location.Longitude,
                    Latitude = location.Latitude,
                    Description = TempDescription,
                    ImagePath = $"location_{location.PhotoNr}.jpg",
                    Name = location.LocationName
                };

                TempDescription.Location = Templocation;

                await _connection.InsertAsync(Templocation);
                await _connection.InsertAsync(TempDescription);
                await _connection.InsertAsync(new RouteComponent {
                    Route = TempRoute,
                    Location = Templocation,
                    Note = location.CommentDutch,
                    RouteOrderNumber = location.routeNr,
                    Visited = false
                });

            }
        }


    }

    private async Task<CompleteRouteContent> ConvertRouteDataToObject() {
        using var stream = await FileSystem.OpenAppPackageFileAsync("Configuration.JSON");
        using var reader = new StreamReader(stream);

        string json = await reader.ReadToEndAsync();

        return JsonSerializer.Deserialize<CompleteRouteContent>(json);
    }

    private async Task addTable(IDatabaseTable databaseTable) {
        await _connection.InsertAsync(databaseTable);
    }

    public void GetDatabaseTableAsync() {
        throw new NotImplementedException();
    }

    public void GetDatabaseItemAsync() {
        throw new NotImplementedException();
    }

    public void UpdateDatabaseItemAsync() {
        throw new NotImplementedException();
    }
}