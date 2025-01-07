using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;

namespace RondjeBreda.Domain.Interfaces;

public interface IMapsAPI
{
    Task GetMapsAPIKey();
    Task<Routeobject> CreateRoute(string originLat, string originLon, string destLat, string destLon);
    List<Location> Decode(string encodedPolyline);
}