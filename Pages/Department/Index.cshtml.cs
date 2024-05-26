using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Localyzer.Models;
using Serilog;
using Localyzer.Context;

namespace Localyzer.Pages.Department
{
    public class IndexModel : PageModel
    {
        DataBaseHandler dbHandler = new();
        public List<UserInfo> userlist;
        public void OnGet()
        {
            dbHandler.GetUserList();
            userlist = dbHandler.userlist.ToList();
        }
    }
}
