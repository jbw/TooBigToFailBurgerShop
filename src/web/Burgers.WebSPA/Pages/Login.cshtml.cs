using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Burgers.WebSPA.Pages
{
    public class LoginModel : PageModel
    {
        public async Task OnGetAsync(string redirectUri)
        {
            await HttpContext.ChallengeAsync(
                           OpenIdConnectDefaults.AuthenticationScheme,
                           new AuthenticationProperties
                           {
                               RedirectUri = redirectUri,
                           });
        }
    }
}
