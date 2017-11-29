using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TrekkingForCharity.Site.Web.Authentication
{
    public class AuthenticationController : Controller
    {
     
        [Route("~/sign-in")]
        public async Task SignIn(string returnUrl = "/")
        {
            //await this.HttpContext.ChallengeAsync("Auth0", new AuthenticationProperties { RedirectUri = returnUrl });
            await this.HttpContext.SignInAsync(new ClaimsPrincipal(new List<ClaimsIdentity>()));
        }

        [Authorize]
        public async Task SignOut()
        {
            await this.HttpContext.SignOutAsync("Auth0", new AuthenticationProperties
            {
                RedirectUri = this.Url.Page("Index")
            });
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}