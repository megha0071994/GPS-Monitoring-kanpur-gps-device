// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Text.Json.Serialization;

namespace kanpur_gps_device.Models;

public class DetailDatum
{
    [JsonPropertyName("vehicle_no")]
    public string VehicleNo { get; set; } = null!;

    [JsonPropertyName("date_time")]
    public string DateTime { get; set; } = null!;

    [JsonPropertyName("latitude")]
    public string Latitude { get; set; } = null!;

    [JsonPropertyName("longitude")]
    public string Longitude { get; set; } = null!;

    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [JsonPropertyName("speed")]
    public string Speed { get; set; } = null!;

    [JsonPropertyName("device_id")]
    public string DeviceId { get; set; } = null!;

    [JsonPropertyName("ignition_status")]
    public string IgnitionStatus { get; set; } = null!;

    [JsonPropertyName("temperature_status ")]
    public string TemperatureStatus { get; set; } = null!;

    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}, {3}, {4}, {5}", DateTime, DeviceId, Latitude, Longitude, Location, Speed);
    }
}

public class ApiResponse
{
    [JsonPropertyName("detail_data")]
    public List<DetailDatum> DetailData { get; set; } = null!;
}

