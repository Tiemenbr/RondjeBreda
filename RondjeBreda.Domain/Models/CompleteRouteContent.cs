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
        public double Latitude { get; set; }

        [JsonProperty("oosterLengte")]
        public double Longitude { get; set; }

        [JsonProperty("naam")]
        public string LocationName { get; set; }

        [JsonProperty("opmerkingNL")]
        public string CommentDutch { get; set; }
        
        [JsonProperty("opmerkingEN")]
        public string CommentEnglish { get; set; }

        public int routeNr { get; set; }
    }

}
