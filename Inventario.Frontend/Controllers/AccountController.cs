using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.Frontend.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl = "/")

        {
            var propierties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri("https://inventarios.helgen.dev/")
                .Build();

            return Challenge(propierties, Auth0Constants.AuthenticationScheme);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = "/"
            },
            CookieAuthenticationDefaults.AuthenticationScheme,
            "Auth0");
        }
    }
}
