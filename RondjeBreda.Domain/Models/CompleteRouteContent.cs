using Newtonsoft.Json;

namespace RondjeBreda.Domain.Models
{

    public class CompleteRouteContent
    {
        public RouteLocation[] HistorischeKilometerRoute { get; set; }
    }

    public class RouteLocation
    {
        [JsonProperty("fotoNr")]
        public string PhotoNr { get; set; }

        [JsonProperty("noorderBreedte")]
        public string Latitude { get; set; }

        [JsonProperty("oosterLengte")]
        public string Longitude { get; set; }

        [JsonProperty("naam")]
        public string LocationName { get; set; }

        [JsonProperty("opmerking")]
        public string Comment { get; set; }

        public int routeNr { get; set; }
    }

}
