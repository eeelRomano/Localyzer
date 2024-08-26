namespace Localyzer.Models.Interfaces
{
    public interface ILoggedDevice
    {
        Guid DeviceID { get; set; }
        string DeviceName { get; set; }
        bool Active { get; set; }
        decimal Latitude { get; set; }
        decimal Longtitude { get; set; }
        string DeviceNr { get; set; }
    }
}
