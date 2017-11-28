using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace TrekkingForCharity.Site.Web.Authentication
{
    public class ConfigureOpenIdConnectOptions : IConfigureOptions<OpenIdConnectOptions>
    {
        private readonly AuthenticationSettings _authenticationSettings;

        public ConfigureOpenIdConnectOptions(IOptions<AuthenticationSettings> authenticationSettings)
        {
            if (authenticationSettings == null)
            {
                throw new ArgumentNullException(nameof(authenticationSettings));
            }

            this._authenticationSettings = authenticationSettings.Value;
        }
        public void Configure(OpenIdConnectOptions options)
        {
            
            options.Authority = $"https://{this._authenticationSettings.Domain}";
            options.ClientId = this._authenticationSettings.ClientId;
            options.ClientSecret = this._authenticationSettings.ClientSecret;
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.CallbackPath = new PathString("/signin-auth0");
            options.ClaimsIssuer = "Auth0";
            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProviderForSignOut = this.OnRedirectToIdentityProviderForSignOut
            };
        }

        private async Task OnRedirectToIdentityProviderForSignOut(RedirectContext redirectContext)
        {
            var logoutUri = $"https://{this._authenticationSettings.Domain}/v2/logout?client_id={this._authenticationSettings.ClientId}";

            var postLogoutUri = redirectContext.Properties.RedirectUri;
            if (!string.IsNullOrEmpty(postLogoutUri))
            {
                if (postLogoutUri.StartsWith("/"))
                {
                    // transform to absolute
                    var request = redirectContext.Request;
                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                }

                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
            }

            redirectContext.Response.Redirect(logoutUri);
            redirectContext.HandleResponse();
        }
    }
}