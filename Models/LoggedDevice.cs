using Localyzer.Models.Interfaces;

namespace Localyzer.Models
{
    public class LoggedDevice : ILoggedDevice
    {
        public Guid DeviceID { get; set; }
        public string DeviceName { get; set; }
        public bool Active { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
        public string DeviceNr { get; set; }
    }
}
