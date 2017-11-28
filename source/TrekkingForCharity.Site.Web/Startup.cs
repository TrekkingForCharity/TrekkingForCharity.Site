using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TrekkingForCharity.Site.Web.Authentication;
using TrekkingForCharity.Site.Web.Integrations.Doorbell;

namespace TrekkingForCharity.Site.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddMvc()
            .AddRazorPagesOptions(options => {
                options.Conventions.AddPageRoute("/TermsAndPrivacy", "terms-and-privacy");
            });
            services.Configure<AuthenticationSettings>(this.Configuration.GetSection("auth0"));
            services.Configure<DoorbellSettings>(this.Configuration.GetSection("doorbell"));
            

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddOpenIdConnect("Auth0", options =>
                {
                    options.Authority = $"https://{this.Configuration["Auth0:Domain"]}";

                    options.ClientId = this.Configuration["Auth0:ClientId"];
                    options.ClientSecret = this.Configuration["Auth0:ClientSecret"];

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
                });
            services.AddScoped<IDoorbellClient, DoorbellClient>();
        }

        private async Task OnRedirectToIdentityProviderForSignOut(RedirectContext redirectContext)
        {
            var logoutUri =
                $"https://{this.Configuration["Auth0:Domain"]}/v2/logout?client_id={this.Configuration["Auth0:ClientId"]}";

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



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
