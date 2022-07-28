using System.Globalization;
using kanpur_gps_device.Services;
using kanpur_gps_device.Models;

TimeZoneInfo TimeZoneInfoIST = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

// Console.WriteLine("Bansal Devices Parser Program Started!");

var _client = new HttpClient();
var timer = new PeriodicTimer(TimeSpan.FromMinutes(3));
List<DetailDatum> old_data = new List<DetailDatum>();
while (await timer.WaitForNextTickAsync())
{
    Console.WriteLine("Fetching Data From Bansal Api At {0}", DateTime.Now);
    var data = await BansalDevicesApi.GetData(_client);
    if (data != null)
    {
        foreach (var item in data)
        {
            double dist = 0;
            if(old_data != null){
                foreach (var item1 in old_data)
                {
                    if(item.DeviceId == item1.DeviceId){
                        // start distance calculation
                        var lat1 = Convert.ToDouble(item.Latitude);
                        var lon1 = Convert.ToDouble(item.Longitude);
                        var lat2 = Convert.ToDouble(item1.Latitude);
                        var lon2 = Convert.ToDouble(item1.Longitude);
                        double rlat1 = Math.PI*lat1/180;
                        double rlat2 = Math.PI*lat2/180;
                        double theta = lon1 - lon2;
                        double rtheta = Math.PI*theta/180;
                        dist = (Math.Sin(rlat1)*Math.Sin(rlat2) )+ (Math.Cos(rlat1)*Math.Cos(rlat2)*Math.Cos(rtheta));
                        dist = Math.Acos(dist);
                        dist = dist*180/Math.PI;
                        dist = dist*60*1.1515;
                        dist = dist*1.609344; // dist in km
                        // end distance calculation
                        Console.WriteLine(item1.DeviceId + " = " + item.Latitude + "-" + item.Longitude + " = " + item1.Latitude + "-" + item1.Longitude + " <=> " + dist);
                        Console.WriteLine("Previous Data = " + item1.ToString());
                        Console.WriteLine("New Data = " + item.ToString());
                        Console.WriteLine("Distance = " + dist + " Km");
                        Console.WriteLine(" ---- *** ---- ");
                        break;
                    }
                }
            }
            // var convert_date = DateTime.Parse(item.DateTime, new CultureInfo("en-GB"));
            // await GpsMonitorerApiClient.ProcessLogin(item.DeviceId, _client);
            // Console.WriteLine(convert_date.ToUniversalTime());
            // await GpsMonitorerApiClient.AddLocationPackets(item.DeviceId, new List<LocationPacket>{ new LocationPacket{
            //     Latitude = Convert.ToDouble(item.Latitude),
            //     Longitude = Convert.ToDouble(item.Longitude),
                // event_at = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(convert_date), TimeZoneInfoIST),
            //     event_at = convert_date.ToUniversalTime(),
            //     Speed = Convert.ToInt16(item.Speed),
            //     TripOdometer =  (dist.ToString() != null)?Convert.ToInt16(dist):0,
            //     IgnitionStatus = item.IgnitionStatus == "0" ? IgnitionState.OFF : IgnitionState.ON
            // }}, _client);
        }
        old_data = data;
    }
}