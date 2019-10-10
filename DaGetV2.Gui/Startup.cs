namespace DaGetV2.Gui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OAuth;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));
            var conf = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "DaOAuth";
            })
           .AddCookie(options =>
           {
               options.Events.OnValidatePrincipal = context =>
               {
                   if (context.Principal.Identity.IsAuthenticated)
                   {
                       var tokens = context.Properties.GetTokens();
                       var exp = tokens.FirstOrDefault(t => t.Name == "expires_at");
                       var expires = DateTime.Parse(exp.Value);

                       if (expires < DateTime.Now.AddSeconds(+120))
                       {
                           context.RejectPrincipal();
                           return Task.CompletedTask;
                       }
                   }
                   return Task.CompletedTask;
               };
           })
           .AddOAuth<OAuthOptions, DaOAuthHandler<OAuthOptions>>("DaOAuth", options =>
           {
               options.AuthorizationEndpoint = conf.AuthorizeEndpoint;
               options.ClientId = conf.ClientId;
               options.ClientSecret = conf.ClientSecret;
               options.CallbackPath = new PathString("/authorization-code/callback");
               options.TokenEndpoint = conf.TokenEndpoint;

               options.Scope.Add("RW_bank_account");
               options.Scope.Add("RW_operation");
               options.Scope.Add("RW_operation_type");

               options.SaveTokens = true;

               options.Events = new OAuthEvents
               {
                   OnCreatingTicket = async context =>
                   {
                       context.Principal.AddIdentity(new System.Security.Claims.ClaimsIdentity(new List<Claim>()
                       {
                           new Claim("access_token", context.AccessToken)
                       }));

                       await Task.CompletedTask;
                   }
               };
           });

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new CustomBinderProvider());
                options.Filters.Add(new UnauthorizedHandler());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
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
