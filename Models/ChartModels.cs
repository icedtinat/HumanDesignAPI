using System.ComponentModel.DataAnnotations;

namespace HumanDesignAPI.Models
{
    public class ChartRequest
    {
        [Required]
        public DateTime BirthDateTime { get; set; }
        
        [Required]
        public string TimeZone { get; set; } = "UTC";
        
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class ChartResponse
    {
        public string Type { get; set; } = "";
        public string Profile { get; set; } = "";
        public int[] Gates { get; set; } = Array.Empty<int>();
        public string[] Channels { get; set; } = Array.Empty<string>();
        public string[] DefinedCenters { get; set; } = Array.Empty<string>();
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class TransitRequest
    {
        [Required]
        public DateTime Date { get; set; }
        
        public string TimeZone { get; set; } = "UTC";
    }

    public class TransitResponse
    {
        public DateTime Date { get; set; }
        public int[] ActiveGates { get; set; } = Array.Empty<int>();
        public string[] ActiveChannels { get; set; } = Array.Empty<string>();
        public string Summary { get; set; } = "";
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class ChartDTO
    {
        public string Type { get; set; }
        public string Profile { get; set; }
        public int[] Gates { get; set; }
        public string[] Channels { get; set; }
        public string[] DefinedCenters { get; set; }

        public ChartDTO(string type, string profile, int[] gates, string[] channels, string[] definedCenters)
        {
            Type = type;
            Profile = profile;
            Gates = gates;
            Channels = channels;
            DefinedCenters = definedCenters;
        }
    }
}
