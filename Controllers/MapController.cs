using Localyzer.Context;
using Localyzer.Models;
using Localyzer.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Localyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : Controller
    {
        readonly DataBaseHandler? dbhandler;
        List<DeviceLocation> devices = [];
        Guid Logged;
        private readonly ILoggedDevice _logged;
        public MapController(ILoggedDevice logged)
        {
            _logged = logged;
            dbhandler = new(_logged);
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceLocation>> GetDeviceLocations()
        {
            try
            {
                //pobranie pozycji aktywnych urządzeń z bazy
                dbhandler.GetUserList();
                var userid = HttpContext.Session.GetString("userid");

                Guid test = Guid.Parse("00000000-0000-0000-0000-000000000000");
                if (_logged.DeviceID != test)
                    Debug.WriteLine(_logged.DeviceID);

                devices = dbhandler.GetDeviceList();
                int userAmount = dbhandler.userlist.Count;
                if (dbhandler.userlist.Count != 0 && userid != null)
                {
                    int index = Convert.ToInt32(userid);
                    _logged.DeviceName = dbhandler.userlist[index].name;
                }

                var locations = new List<MarkerLocation>();
                for (int i = 0; i < userAmount; i++)
                {
                    double latitude = Convert.ToDouble(devices[i].Latitude, CultureInfo.InvariantCulture);
                    double longtitude = Convert.ToDouble(devices[i].Longtitude, CultureInfo.InvariantCulture);

                    MarkerLocation location = new MarkerLocation
                    {
                        DeviceId = dbhandler.userlist[Convert.ToInt32(devices[i].DeviceNr)].name,
                        Latitude = latitude,
                        Longitude = longtitude,
                        LoggedID = _logged.DeviceName,
                        DeviceNr = devices[i].DeviceNr,
                    };

                    if (devices[i].Active)
                        locations.Add(location);
                }
                return Ok(locations);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); return Ok(); };
        }

        [HttpPost("update")]
        public IActionResult UpdateLocation([FromBody] DeviceLocationModel location)
        {
            try
            {
                // Retrieve the user ID from the request
                var userId = location.UserId;

                if (location == null || string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Location data or user ID is null.");
                }

                // Process the location data, e.g., save it to the database, log it, etc.
                if(userId == "1")
                Debug.WriteLine($"Received location for user {userId}: Latitude = {location.Latitude}, Longitude = {location.Longitude}");

                // Update the database with the location data
                dbhandler.UpdateDeviceLocations(location.Latitude, location.Longitude, userId);

                return Ok(new { message = "Location updated successfully" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return BadRequest("An error occurred while updating the location.");
            }
        }

        public class DeviceLocationModel
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string UserId { get; set; }
        }
    }
}
