using System.Text.Json;
using kanpur_gps_device.Models;

namespace kanpur_gps_device.Services;

public static class BansalDevicesApi
{
    public static readonly string _url = "http://13.234.242.1/jsp/Service_vehicle.jsp?user=bansal01&pass=123456";
    public static async Task<List<DetailDatum>?> GetData(HttpClient client)
    {
        var res = await client.GetAsync(_url);
        res.EnsureSuccessStatusCode();
        var resJson = JsonSerializer.Deserialize<ApiResponse>(await res.Content.ReadAsStringAsync());
        if(resJson == null) {
            return default;
        }
        return resJson.DetailData;
    }
}