using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Localyzer.Models;
using Serilog;
using Localyzer.Context;
using Localyzer.Models.Interfaces;

namespace Localyzer.Pages.Department
{
    public class IndexModel : PageModel
    {
        DataBaseHandler? dbHandler;
        ILoggedDevice logged;
        public List<UserInfo> userlist;
        public IndexModel(ILoggedDevice loged)
        {
            logged = loged;
            dbHandler = new(logged);
        }
        public void OnGet()
        {
            dbHandler.GetUserList();
            userlist = dbHandler.userlist.ToList();
        }
    }
}
