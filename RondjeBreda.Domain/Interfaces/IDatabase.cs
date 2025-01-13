using RondjeBreda.Domain.Models.DatabaseModels;

namespace RondjeBreda.Domain.Interfaces;

/// <summary>
/// Interface for the database connection
/// </summary>
public interface IDatabase
{
    Task Init();
    Task<Route[]> GetRouteTableAsync();
    Task<Location[]> GetLocationTableAsync();
    Task<Description[]> GetDescriptionTableAsync();
    Task<RouteComponent[]> GetRouteComponentTableAsync();
    Task<RouteComponent[]> GetRouteComponentFromRouteAsync(string routeName);
    Task<Route> GetRouteAsync(string routeName);
    Task<Location> GetLocationAsync(double longitude, double latitude);
    Task<Description> GetDescriptionAsync(string descriptionNL);
    Task<RouteComponent> GetRouteComponentAsync(string routeName, double longitude, double latitude);
    Task UpdateRoute(string name, bool newActiveState);
    Task UpdateRouteComponent(string routeName, double longitude, double latitude, bool newIsVisited);

}