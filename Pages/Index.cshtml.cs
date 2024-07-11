using Localyzer.Context;
using Localyzer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Localyzer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        DataBaseHandler dbHandler = new();
        UserInfo? user;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost() 
        {
            var login = Request.Form["login"];
            var password = Request.Form["password"];

            DBlogin(login, password);
        }

        private IActionResult DBlogin(string login, string password) 
        {
            String userid = dbHandler.Login(login, password);
            if (userid == null || userid == "0" || userid=="" || userid==" ")
            {
                ViewData["Error"] = "Nieprawid³owy login lub has³o.";
                return Page();
            }

            TempData["UserID"] = userid;
            Response.Redirect("/Home/Index");
            return Page();
        }
    }
}
