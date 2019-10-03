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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DaGetV2.Blazor.Server
{
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

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseClientSideBlazorFiles<Wasm.Startup>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToClientSideBlazor<Wasm.Startup>("index.html");
            });
        }
    }
}
