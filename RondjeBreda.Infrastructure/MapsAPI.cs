using RondjeBreda.Domain.Interfaces;

using System.Diagnostics;
using System.Net.Http.Json;
using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;
using Microsoft.Maui.Devices.Sensors;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace RondjeBreda.Infrastructure;

public class MapsAPI : IMapsAPI
{
    private string MapsAPIKey;

    public Task GetMapsAPIKey()
    {
        throw new NotImplementedException();
    }

    public async Task<Routeobject> CreateRoute(string originLat, string originLon, string destLat, string destLon)
    {
        if (MapsAPIKey == null)
        {
            return null;
        }

        ConvertCommasToDot(ref originLat, ref originLon, ref destLat, ref destLon);

        string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={originLat},{originLon}&destination={destLat},{destLon}&mode=walking&key={MapsAPIKey}";
        Debug.WriteLine($"URL: {url}");

        using HttpClient client = new HttpClient();
        var response = await client.GetFromJsonAsync<Routeobject>(url);

        return response;
    }

    public List<Location> Decode(string encodedPolyline)
    {
        var polyline = new List<Location>();
        int index = 0, lat = 0, lng = 0;

        // Loop door de gehele encodedPolyline string
        while (index < encodedPolyline.Length)
        {
            int b, shift = 0, result = 0;
            // Decodeer de latitude (lat)
            do
            {
                b = encodedPolyline[index++] - 63;
                result |= (b & 0x1f) << shift;
                shift += 5;
            } while (b >= 0x20);

            // Bereken de delta (verschil) voor latitude
            int deltaLat = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
            lat += deltaLat;

            // Reset voor longitude (lng)
            shift = 0;
            result = 0;

            // Decodeer de longitude (lng)
            do
            {
                b = encodedPolyline[index++] - 63;
                result |= (b & 0x1f) << shift;
                shift += 5;
            } while (b >= 0x20);

            // Bereken de delta (verschil) voor longitude
            int deltaLng = ((result & 1) != 0 ? ~(result >> 1) : (result >> 1));
            lng += deltaLng;

            // Voeg de nieuwe coördinaten toe aan de polyline lijst
            var location = new Location(lat * 1e-5, lng * 1e-5);
            polyline.Add(location);
        }

        return polyline;
    }

    private void ConvertCommasToDot(ref string originLat, ref string originLon, ref string destLat, ref string destLon)
    {
        originLat = originLat.Replace(",", ".");
        originLon = originLon.Replace(",", ".");
        destLat = destLat.Replace(",", ".");
        destLon = destLon.Replace(",", ".");
    }
}