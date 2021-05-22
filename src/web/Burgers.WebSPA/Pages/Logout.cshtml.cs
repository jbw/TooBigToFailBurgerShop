using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Burgers.WebSPA.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<RedirectResult> OnGetAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }   
    }
}
