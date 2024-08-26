namespace Localyzer.Models
{
    public class MarkerLocation
    {
        public string DeviceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LoggedID { get; set; }
        public int DeviceNr { get; set; }
        public MarkerLocation(string deviceId, double latitude, double longitude, string loggedID, int deviceNr)
        {
            DeviceId = deviceId;
            Latitude = latitude;
            Longitude = longitude;
            LoggedID = loggedID;
            DeviceNr = deviceNr;
        }

        public MarkerLocation() { }
    }
}
