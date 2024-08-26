using Localyzer.Context;
using Localyzer.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics;

namespace Localyzer.Controllers
{
    public class HomeController : Controller
    {
        public bool Active { get; set; }
        private readonly DataBaseHandler? _dbhandler;
        private readonly ILoggedDevice _logged;
        public HomeController(ILoggedDevice logged)
        {
            _logged = logged;
            _dbhandler = new(_logged);
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                _dbhandler.GetDeviceList();
                bool status = _dbhandler.CheckDeviceStatus(_logged.DeviceNr);
                TempData["status"] = "Nieaktywny";
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return View();
        }

        [HttpPost]
        public IActionResult ActivateLocating() 
        {
            try
            {
                string? userid = HttpContext.Session.GetString("userid");
                Active = _dbhandler.CheckDeviceStatus(userid);

                if (!Active)
                {
                    Active = true;
                    _dbhandler?.ActivateDeviceLocating(userid ?? "");
                    TempData["status"] = "Aktywny";
                    //HttpContext.Session.SetString("status","Aktywny");
                }
                else
                {
                    Active = false;
                    _dbhandler?.ActivateDeviceLocating(userid ?? "");
                    TempData["status"] = "Nieaktywny";
                    //HttpContext.Session.SetString("status", "Niektywny");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message);};

            return Redirect("/Home/Index");
        }
    }
}
