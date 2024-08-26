using System;

namespace Localyzer.Models
{
    public class DeviceLocation
    {
        public Guid DeviceId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
        public bool Active { get; set; }
        public int DeviceNr { get; set; }
        public DeviceLocation(Guid deviceId, decimal latitude, decimal longitude, bool active, int devicenr) 
        { 
            DeviceId = deviceId;
            Latitude = latitude;
            Longtitude = longitude;
            Active = active;
            DeviceNr = devicenr;
        }

        public DeviceLocation() { }
    }
}
