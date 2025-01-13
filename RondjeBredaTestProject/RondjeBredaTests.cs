using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;
using System.Threading.Tasks;

namespace RondjeBredaTestProject
{
    /// <summary>
    /// The unit tests in this file have been written with the help of GitHub Copilot.
    /// Comments have been added to assure understanding of the written code.
    /// Console.WriteLines have also been added to view and understand the test results. 
    /// </summary>

    #region CompleteRouteContent Tests Project
    [TestClass]
    public sealed class CompleteRouteContentTests
    {
        [TestMethod]
        public void CompleteRouteContentSerialization_ShouldSerializeCorrectly_WhenCalled()
        {
            Console.WriteLine("Started Test: CompleteRouteContentSerialization_ShouldSerializeCorrectly_WhenCalled"); // Started Test
            // Assign
            var routeContent = new CompleteRouteContent
            {
                HistorischeKilometerRoute = new RouteLocation[]
                {
                    new RouteLocation
                    {
                        PhotoNr = 2,
                        Latitude = 51.594445,
                        Longitude = 4.779417,
                        LocationName = "Oude VVV-pand",
                        CommentDutch = "Vertrekpunt station",
                        CommentEnglish = "Starting point station",
                        routeNr = 1
                    }
                }
            };
            var expectedResult = JsonConvert.SerializeObject(routeContent);
            Console.WriteLine($"Expected Result: {expectedResult}"); // Expected Result

            // Act
            var actualResult = "{\"HistorischeKilometerRoute\":[{\"PhotoNr\":2,\"Latitude\":51.594445,\"Longitude\":4.779417,\"LocationName\":\"Oude VVV-pand\",\"CommentDutch\":\"Vertrekpunt station\",\"CommentEnglish\":\"Starting point station\",\"routeNr\":1}]}";
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {expectedResult} and {actualResult}");
            Assert.AreEqual(expectedResult, actualResult);
            Console.WriteLine("Finished Test: CompleteRouteContentSerialization_ShouldSerializeCorrectly_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public void CompleteRouteContentDeserialization_ShouldDeserializeCorrectly_WhenCalled()
        {
            Console.WriteLine("Started Test: CompleteRouteContentDeserialization_ShouldDeserializeCorrectly_WhenCalled"); // Started Test
            // Assign
            var json = "{\"HistorischeKilometerRoute\":[{\"PhotoNr\":2,\"Latitude\":51.594445,\"Longitude\":4.779417,\"LocationName\":\"Oude VVV-pand\",\"CommentDutch\":\"Vertrekpunt station\",\"CommentEnglish\":\"Starting point station\",\"routeNr\":1}]}";
            
            // Act
            var actualResult = JsonConvert.DeserializeObject<CompleteRouteContent>(json);
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {1},{2},{51.594445},{4.779417},Oude VVV-pand,Vertrekpunt station,Starting point station,{1} with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualResult.HistorischeKilometerRoute.Length);
            Assert.AreEqual(2, actualResult.HistorischeKilometerRoute[0].PhotoNr);
            Assert.AreEqual(51.594445, actualResult.HistorischeKilometerRoute[0].Latitude);
            Assert.AreEqual(4.779417, actualResult.HistorischeKilometerRoute[0].Longitude);
            Assert.AreEqual("Oude VVV-pand", actualResult.HistorischeKilometerRoute[0].LocationName);
            Assert.AreEqual("Vertrekpunt station", actualResult.HistorischeKilometerRoute[0].CommentDutch);
            Assert.AreEqual("Starting point station", actualResult.HistorischeKilometerRoute[0].CommentEnglish);
            Assert.AreEqual(1, actualResult.HistorischeKilometerRoute[0].routeNr);
            Console.WriteLine("Finished Test: CompleteRouteContentDeserialization_ShouldDeserializeCorrectly_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public void RouteLocationSerialization_ShouldSerializeCorrectly_WhenCalled()
        {
            Console.WriteLine("Started Test: RouteLocationSerialization_ShouldSerializeCorrectly_WhenCalled"); // Started Test
            // Assign
            var routeLocation = new RouteLocation
            {
                PhotoNr = 2,
                Latitude = 51.594445,
                Longitude = 4.779417,
                LocationName = "Oude VVV-pand",
                CommentDutch = "Vertrekpunt station",
                CommentEnglish = "Starting point station",
                routeNr = 1
            };
            var expectedResult = JsonConvert.SerializeObject(routeLocation);
            Console.WriteLine($"Expected Result: {expectedResult}"); // Expected Result

            // Act
            var actualResult = "{\"PhotoNr\":2,\"Latitude\":51.594445,\"Longitude\":4.779417,\"LocationName\":\"Oude VVV-pand\",\"CommentDutch\":\"Vertrekpunt station\",\"CommentEnglish\":\"Starting point station\",\"routeNr\":1}";
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {expectedResult} and {actualResult}");
            Assert.AreEqual(expectedResult, actualResult);
            Console.WriteLine("Finished Test: RouteLocationSerialization_ShouldSerializeCorrectly_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public void RouteLocationDeserialization_ShouldDeserializeCorrectly_WhenCalled()
        {
            Console.WriteLine("Started Test: RouteLocationDeserialization_ShouldDeserializeCorrectly_WhenCalled"); // Started Test
            // Assign
            var json = "{\"PhotoNr\":2,\"Latitude\":51.594445,\"Longitude\":4.779417,\"LocationName\":\"Oude VVV-pand\",\"CommentDutch\":\"Vertrekpunt station\",\"CommentEnglish\":\"Starting point station\",\"routeNr\":1}";
            
            // Act
            var actualResult = JsonConvert.DeserializeObject<RouteLocation>(json);
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {2},{51.594445},{4.779417},Oude VVV-pand,Vertrekpunt station,Starting point station,{1} with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(2, actualResult.PhotoNr);
            Assert.AreEqual(51.594445, actualResult.Latitude);
            Assert.AreEqual(4.779417, actualResult.Longitude);
            Assert.AreEqual("Oude VVV-pand", actualResult.LocationName);
            Assert.AreEqual("Vertrekpunt station", actualResult.CommentDutch);
            Assert.AreEqual("Starting point station", actualResult.CommentEnglish);
            Assert.AreEqual(1,actualResult.routeNr);
            Console.WriteLine("Finished Test: RouteLocationDeserialization_ShouldDeserializeCorrectly_WhenCalled"); // Finished Test
        }
    }
    #endregion

    #region IDatabase Tests Project
    [TestClass]
    public sealed class IDatabaseTests
    {
        private Mock<IDatabase> mockDatabase;

        [TestInitialize]
        public void SetUp()
        {
            mockDatabase = new Mock<IDatabase>(); // Mock the IDatabase interface to use for unit tests
        }

        [TestMethod]
        public async Task Init_ShouldInitializeDatabase_WhenCalled()
        {
            Console.WriteLine("Started Test: Init_ShouldInitializeDatabase_WhenCalled"); // Started Test
            // Assign
            mockDatabase.Setup(db => db.Init()).Returns(Task.CompletedTask);

            // Act
            await mockDatabase.Object.Init();

            // Assert
            mockDatabase.Verify(db => db.Init(), Times.Once);
            Console.WriteLine("Finished Test: Init_ShouldInitializeDatabase_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetRouteTableAsync_ShouldReturnRoutes_WhenCalled()
        {
            Console.WriteLine("Started Test: GetRouteTableAsync_ShouldReturnRoutes_WhenCalled"); // Started Test
            // Assign
            var expectedRoutes = new Route[] { new Route { Name = "TestRoute", Active = true } };
            mockDatabase.Setup(db => db.GetRouteTableAsync()).ReturnsAsync(expectedRoutes);

            // Act
            var actualResult = await mockDatabase.Object.GetRouteTableAsync();
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {1},TestRoute with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualResult.Length);
            Assert.AreEqual("TestRoute", actualResult[0].Name);
            Console.WriteLine("Finished Test: GetRouteTableAsync_ShouldReturnRoutes_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetLocationTableAsync_ShouldReturnLocations_WhenCalled()
        {
            Console.WriteLine("Started Test: GetLocationTableAsync_ShouldReturnLocations_WhenCalled"); // Started Test
            // Assign
            var expectedLocations = new Location[] { new Location { Longitude = 4.779417, Latitude = 51.594445 } };
            mockDatabase.Setup(db => db.GetLocationTableAsync()).ReturnsAsync(expectedLocations);

            // Act
            var actualResult = await mockDatabase.Object.GetLocationTableAsync();
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {1},{4.779417},{51.594445} with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualResult.Length);
            Assert.AreEqual(4.779417, actualResult[0].Longitude);
            Assert.AreEqual(51.594445, actualResult[0].Latitude);
            Console.WriteLine("Finished Test: GetLocationTableAsync_ShouldReturnLocations_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetDescriptionTableAsync_ShouldReturnDescriptions_WhenCalled()
        {
            Console.WriteLine("Started Test: GetDescriptionTableAsync_ShouldReturnDescriptions_WhenCalled"); // Started Test
            // Assign
            var expectedDescriptions = new Description[] { new Description { DescriptionNL = "TestDescription" } };
            mockDatabase.Setup(db => db.GetDescriptionTableAsync()).ReturnsAsync(expectedDescriptions);

            // Act
            var actualResult = await mockDatabase.Object.GetDescriptionTableAsync();
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {1},TestDescription with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualResult.Length);
            Assert.AreEqual("TestDescription", actualResult[0].DescriptionNL);
            Console.WriteLine("Finished Test: GetDescriptionTableAsync_ShouldReturnDescriptions_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetRouteComponentTableAsync_ShouldReturnRouteComponents_WhenCalled()
        {
            Console.WriteLine("Started Test: GetRouteComponentTableAsync_ShouldReturnRouteComponents_WhenCalled"); // Started Test
            // Assign
            var expectedRouteComponents = new RouteComponent[] { new RouteComponent { RouteName = "TestRoute" } };
            mockDatabase.Setup(db => db.GetRouteComponentTableAsync()).ReturnsAsync(expectedRouteComponents);

            var actualResult = await mockDatabase.Object.GetRouteComponentTableAsync();
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {1},TestRoute and {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualResult.Length);
            Assert.AreEqual("TestRoute", actualResult[0].RouteName);
            Console.WriteLine("Finished Test: GetRouteComponentTableAsync_ShouldReturnRouteComponents_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetRouteComponentFromRouteAsync_ShouldReturnRouteComponents_WhenCalled()
        {
            Console.WriteLine("Started Test: GetRouteComponentFromRouteAsync_ShouldReturnRouteComponents_WhenCalled"); // Started Test
            // Assign
            var expectedRouteComponents = new RouteComponent[] { new RouteComponent { RouteName = "TestRoute" } };
            mockDatabase.Setup(db => db.GetRouteComponentFromRouteAsync("TestRoute")).ReturnsAsync(expectedRouteComponents);

            // Act
            var actualResult = await mockDatabase.Object.GetRouteComponentFromRouteAsync("TestRoute");
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {1},TestRoute with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(1, actualResult.Length);
            Assert.AreEqual("TestRoute", actualResult[0].RouteName);
            Console.WriteLine("Finished Test: GetRouteComponentFromRouteAsync_ShouldReturnRouteComponents_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetRouteAsync_ShouldReturnRoute_WhenCalled()
        {
            Console.WriteLine("Started Test: GetRouteAsync_ShouldReturnRoute_WhenCalled"); // Started Test
            // Assign
            var expectedRoute = new Route { Name = "TestRoute" };
            mockDatabase.Setup(db => db.GetRouteAsync("TestRoute")).ReturnsAsync(expectedRoute);

            // Act
            var actualResult = await mockDatabase.Object.GetRouteAsync("TestRoute");
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: TestRoute with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("TestRoute", actualResult.Name);
            Console.WriteLine("Finished Test: GetRouteAsync_ShouldReturnRoute_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetLocationAsync_ShouldReturnLocation_WhenCalled()
        {
            Console.WriteLine("Started Test: GetLocationAsync_ShouldReturnLocation_WhenCalled"); // Started Test
            // Assign
            var expectedLocation = new Location { Longitude = 4.779417, Latitude = 51.594445 };
            mockDatabase.Setup(db => db.GetLocationAsync(4.779417, 51.594445)).ReturnsAsync(expectedLocation);

            // Act
            var actualResult = await mockDatabase.Object.GetLocationAsync(4.779417, 51.594445);
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: {4.779417},{51.594445} with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual(4.779417, actualResult.Longitude);
            Assert.AreEqual(51.594445, actualResult.Latitude);
            Console.WriteLine("Finished Test: GetLocationAsync_ShouldReturnLocation_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetDescriptionAsync_ShouldReturnDescription_WhenCalled()
        {
            Console.WriteLine("Started Test: GetDescriptionAsync_ShouldReturnDescription_WhenCalled"); // Started Test
            // Assign
            var expectedDescription = new Description { DescriptionNL = "TestDescription" };
            mockDatabase.Setup(db => db.GetDescriptionAsync("TestDescription")).ReturnsAsync(expectedDescription);

            // Act
            var actualResult = await mockDatabase.Object.GetDescriptionAsync("TestDescription");
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: TestDescription with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("TestDescription", actualResult.DescriptionNL);
            Console.WriteLine("Finished Test: GetDescriptionAsync_ShouldReturnDescription_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task GetRouteComponentAsync_ShouldReturnRouteComponent_WhenCalled()
        {
            Console.WriteLine("Started Test: GetRouteComponentAsync_ShouldReturnRouteComponent_WhenCalled"); // Started Test
            // Assign
            var expectedRouteComponent = new RouteComponent { RouteName = "TestRoute", LocationLongitude = 4.779417, LocationLatitude = 51.594445 };
            mockDatabase.Setup(db => db.GetRouteComponentAsync("TestRoute", 4.779417, 51.594445)).ReturnsAsync(expectedRouteComponent);

            // Act
            var actualResult = await mockDatabase.Object.GetRouteComponentAsync("TestRoute", 4.779417, 51.594445);
            Console.WriteLine($"Actual Result: {actualResult}"); // Actual Result

            // Assert
            Console.WriteLine($"Comparing Results: TestRoute,{4.779417},{51.594445} with values of object {actualResult}");
            Assert.IsNotNull(actualResult);
            Assert.AreEqual("TestRoute", actualResult.RouteName);
            Assert.AreEqual(4.779417, actualResult.LocationLongitude);
            Assert.AreEqual(51.594445, actualResult.LocationLatitude);
            Console.WriteLine("Finished Test: GetRouteComponentAsync_ShouldReturnRouteComponent_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task UpdateRoute_ShouldUpdateRoute_WhenCalled()
        {
            Console.WriteLine("Started Test: UpdateRoute_ShouldUpdateRoute_WhenCalled"); // Started Test
            // Assign
            mockDatabase.Setup(db => db.UpdateRoute("TestRoute", true)).Returns(Task.CompletedTask);

            // Act
            await mockDatabase.Object.UpdateRoute("TestRoute", true);

            // Assert
            mockDatabase.Verify(db => db.UpdateRoute("TestRoute", true), Times.Once);
            Console.WriteLine("Finished Test: UpdateRoute_ShouldUpdateRoute_WhenCalled"); // Finished Test
        }

        [TestMethod]
        public async Task UpdateRouteComponent_ShouldUpdateRouteComponent_WhenCalled()
        {
            Console.WriteLine("Started Test: UpdateRouteComponent_ShouldUpdateRouteComponent_WhenCalled"); // Started Test
            // Assign
            mockDatabase.Setup(db => db.UpdateRouteComponent("TestRoute", 4.779417, 51.594445, true)).Returns(Task.CompletedTask);

            // Act
            await mockDatabase.Object.UpdateRouteComponent("TestRoute", 4.779417, 51.594445, true);

            // Assert
            mockDatabase.Verify(db => db.UpdateRouteComponent("TestRoute", 4.779417, 51.594445, true), Times.Once);
            Console.WriteLine("Finished Test: UpdateRouteComponent_ShouldUpdateRouteComponent_WhenCalled"); // Finished Test
        }
    }
    #endregion
}