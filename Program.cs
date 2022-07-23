// See https://aka.ms/new-console-template for more information
using kanpur_gps_device.Services;

TimeZoneInfo TimeZoneInfoIST =
        TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

Console.WriteLine("Bansal Devices Parser Program Started!");

var _client = new HttpClient();
var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

while (await timer.WaitForNextTickAsync())
{
    Console.WriteLine("Fetching Data From Bansal Api At {0}", DateTime.Now);
    var data = await BansalDevicesApi.GetData(_client);
    if (data != null)
    {
        foreach (var item in data)
        {
            Console.WriteLine(item.ToString());
            await GpsMonitorerApiClient.ProcessLogin(item.DeviceId, _client);
            await GpsMonitorerApiClient.AddLocationPackets(item.DeviceId, new List<LocationPacket>{ new LocationPacket{
                Latitude = Convert.ToDouble(item.Latitude),
                Longitude = Convert.ToDouble(item.Longitude),
                event_at = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(item.DateTime), TimeZoneInfoIST),
                Speed = Convert.ToInt16(item.Speed),
                IgnitionStatus = item.IgnitionStatus == "0" ? IgnitionState.OFF : IgnitionState.ON
            }}, _client);
        }
    }
}