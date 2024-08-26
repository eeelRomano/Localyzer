using Localyzer.Context;
using Localyzer.Models;
using Localyzer.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Diagnostics;

namespace Localyzer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ILoggedDevice logged;
        DataBaseHandler? dbHandler;
        UserInfo? user;

        public IndexModel(ILogger<IndexModel> logger, ILoggedDevice loged)
        {
            _logger = logger;
            logged = loged;
            dbHandler = new(logged);
        }

        public void OnGet()
        {

        }

        public void OnPost() 
        {
            var login = "";
            var password = "";
            try
            {
                login = Request.Form["login"];
                password = Request.Form["password"];
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            //CreateUser();
            //CreateUser2();
            //CreateUser3();

            if (login != null && password != null)
               DBlogin(login, password);
        }

        private IActionResult DBlogin(string login, string password) 
        {
            String userid = dbHandler.Login(login, password);
            if (userid == null || userid=="" || userid==" ")
            {
                TempData["Error"] = "Nieprawid³owy login lub has³o.";
                return Page();
            }

            HttpContext.Session.SetString("userid", userid);
            Response.Redirect("/Home/Index");
            return Page();
        }

        public void CreateUser()
        {
            int id = 0;
            string name = "Marek";
            string lastname = "Nowak";
            string email = "marek@localyzer.pl";
            string phone = "222333444";
            string adres = "Solna 9, 00-001 Warszawa";
            int depart = 0;
            DateTime created = DateTime.Now;
            string formattedDate = created.ToString("yyyy-MM-dd HH:mm:ss");
            string login = "marekkk";
            string password = "marekkk";
            Guid deviceId = Guid.NewGuid();
            dbHandler.CreateUser(id, name, lastname,
                                email, phone, adres,
                                depart, formattedDate, login,
                                password, deviceId);
        }

        public void CreateUser2()
        {
            int id = 1;
            string name = "Janusz";
            string lastname = "Kowalski";
            string email = "janusz@localyzer.pl";
            string phone = "333444555";
            string adres = "Wodna 2, 00-001 Warszawa";
            int depart = 0;
            DateTime created = DateTime.Now;
            string formattedDate = created.ToString("yyyy-MM-dd HH:mm:ss");
            string login = "janUSZ";
            string password = "john123";
            Guid deviceId = Guid.NewGuid();
            dbHandler.CreateUser(id, name, lastname,
                                email, phone, adres,
                                depart, formattedDate, login,
                                password, deviceId);
        }

        public void CreateUser3()
        {
            int id = 2;
            string name = "Kacper";
            string lastname = "Kochanowski";
            string email = "kacper@localyzer.pl";
            string phone = "444555666";
            string adres = "Eukaliptusowa 5, 00-001 Warszawa";
            int depart = 0;
            DateTime created = DateTime.Now;
            string formattedDate = created.ToString("yyyy-MM-dd HH:mm:ss");
            string login = "admin";
            string password = "admin";
            Guid deviceId = Guid.NewGuid();
            dbHandler.CreateUser(id, name, lastname,
                                email, phone, adres,
                                depart, formattedDate, login,
                                password, deviceId);
        }
    }
}
