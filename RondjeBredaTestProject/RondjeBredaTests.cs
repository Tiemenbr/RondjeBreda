using Microsoft.Maui.Storage;
using Moq;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;
using RondjeBreda.Infrastructure.DatabaseImplementation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;
using System.Text.Json;

namespace RondjeBredaTestProject {
    [TestClass]
    public sealed class RondjeBredaTests {

        private Mock<SQLiteDatabase> testDatabase;

        [TestInitialize]
        public async Task SetUp()
        {
            testDatabase = new Mock<SQLiteDatabase>();
        }

        [TestMethod]
        public async Task SQLiteDatabase_ShouldReturnCompleteRouteContentFromJsonFile_WhenCalled()
        {
            Console.WriteLine("Finished Test: SQLiteDatabase_ShouldReturnCompleteRouteContentFromJsonFile_WhenCalled"); // Started Test
            // Setup Mock
            CompleteRouteContent routeContent = new CompleteRouteContent()
            {
                HistorischeKilometerRoute =
                [
                    new RouteLocation
                    {
                        PhotoNr = 2,
                        routeNr = 1,
                        Latitude = 51.594445,
                        Longitude = 4.779417,
                        LocationName = "Oude VVV-pand",
                        CommentDutch = "Vertrekpunt station",
                        CommentEnglish = "Starting point station"
                    },
                     new RouteLocation
                    {
                        PhotoNr = 3,
                        routeNr = 1,
                        Latitude = 51.593278,
                        Longitude = 4.779388,
                        LocationName = "Liefdeszuster",
                        CommentDutch = "1",
                        CommentEnglish = null
                    }
                ]
            };
            var expectedResult = JsonSerializer.Serialize(routeContent);

            // Print Expected Result
            Console.WriteLine($"Expected Result: {expectedResult}");

            // Use Function
            CompleteRouteContent actualResult = testDatabase.Object.ConvertRouteDataToObject().Result; //testDatabase.Object.ConvertRouteDataToObject().Result;
            
            // Print Actual Result
            Console.WriteLine($"Actual Result: {actualResult}");
            
            // Assert
            Assert.AreEqual(expectedResult, JsonSerializer.Serialize(actualResult.HistorischeKilometerRoute));
            Console.WriteLine("Finished Test: SQLiteDatabase_ShouldReturnCompleteRouteContentFromJsonFile_WhenCalled"); // Finished Test
        }
    }
}
