using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Localyzer.Pages.Maps
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            TempData["UserId"] = HttpContext.Session.GetString("userid");
        }
    }
}
