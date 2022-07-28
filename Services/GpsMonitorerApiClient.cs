using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kanpur_gps_device.Services
{
    public static class GpsMonitorerApiClient {
        // private static readonly string _baseUrl = "https://gps.eyeballtech.in/";
        private static readonly string _baseUrl = "https://gpsnew.eyeballtech.com/";

        public static async Task<ResponseToDevice?> ProcessLogin(string imei, HttpClient _client)
        {
            var streamTask = _client.GetStreamAsync($"{_baseUrl}DataIntake/processLogin/{imei}");
            Console.WriteLine($"{_baseUrl}DataIntake/processLogin/{imei}");
            return await JsonSerializer.DeserializeAsync<ResponseToDevice>(await streamTask);
        }

        public static async Task<ResponseToDevice?> AddLocationPackets(string imei, List<LocationPacket> locPkts, HttpClient _client)
        {
            var httpResponse = await _client.PostAsync(
                    $"{_baseUrl}DataIntake/addlocationpackets/{imei}",
                    new StringContent(JsonSerializer.Serialize(locPkts), Encoding.UTF8, "application/json")
                );
            return await JsonSerializer.DeserializeAsync<ResponseToDevice>(await httpResponse.Content.ReadAsStreamAsync());
        }

    }

    public class ResponseToDevice {
        public bool success { get; set; }
        public string? message { get; set; }
    }

    public class LocationPacket {
        public DateTime event_at { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Heading { get; set; }
        public int No_of_Satellite { get; set; }
        public double Altitude { get; set; }
        public int Speed { get; set; }
        public string? GPS_Fixed { get; set; }
        public double Odometer { get; set; }
        public string? Cell_ID { get; set; }
        public string? GSM_Signal_Strength { get; set; }
        public string? Battery_Level { get; set; }
        public string? Analog_Input_1 { get; set; } // 0.15
        public string? Analog_Input_2 { get; set; } // 0.25
        public long TripOdometer { get; set; } // distance calculate
        public double BatteryVoltage { get; set; } // mV
        public double BatteryCurrent { get; set; } // mA
        public int EngineLoad { get; set; } // %
        public int EngineRpm { get; set; } // rpm
        public int CoolantTemperature { get; set; } // Degree C
        public int IntakeTemperature { get; set; } // Degree C
        public int EngineOilTemperature { get; set; } // Degree C
        public int AmbientAirTemperature { get; set; } // Degree C
        public int ThrottlePosition { get; set; } // %
        public int FuelLevel { get; set; } // %
        public double FuelRate { get; set; } // L/100km
        public DeviceState DeviceStatus { get; set; }
        public IgnitionState IgnitionStatus { get; set; }
    }

    public enum DeviceState {
        Dormant,
        Stopped,
        Running,
        Idling
    }
    public enum IgnitionState {
        OFF,
        ON
    }
}