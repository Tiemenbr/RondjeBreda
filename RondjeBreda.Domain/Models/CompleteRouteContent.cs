using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace RondjeBreda.Domain.Models
{
    public class CompleteRouteContent
    {

        [JsonPropertyName("HistorischeKilometerRoute")]
        public RouteLocation[] HistorischeKilometerRoute { get; set; }
    }

    public class RouteLocation
    {
        [JsonPropertyName("fotoNr")]
        public int PhotoNr { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("naam")]
        public string LocationName { get; set; }

        [JsonPropertyName("opmerkingNL")]
        public string CommentDutch { get; set; }
        
        [JsonPropertyName("opmerkingEN")]
        public string CommentEnglish { get; set; }

        [JsonPropertyName("routeNr")]
        public int routeNr { get; set; }
    }

}
