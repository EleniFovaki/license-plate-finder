using System.Text.Json.Serialization;

namespace license_plate_finder.Server.Models
{
    public class VehicleDetails
    {
        [JsonPropertyName("kenteken")]
        public string LicensePlate { get; set; }

        [JsonPropertyName("merk")]
        public string Brand { get; set; }

        [JsonPropertyName("handelsbenaming")]
        public string Type { get; set; }

        [JsonPropertyName("eerste_kleur")]
        public string Color { get; set; }

        [JsonPropertyName("aantal_deuren")]
        public string NumberofDoors { get; set; }

        [JsonPropertyName("motorcode")]
        public string MotorCode { get; set; }

        [JsonPropertyName("vermogen_motor_pk")]
        public int BreakHorsePower { get; set; }
    }
}
