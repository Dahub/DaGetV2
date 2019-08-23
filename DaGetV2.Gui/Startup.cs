using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DaGetV2.Gui
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
            services.Configure<AppConfiguration>(Configuration.GetSection("AppConfiguration"));
            var conf = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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

                       if (expires < DateTime.Now.AddSeconds(-120))
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

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

    public class DaOAuthHandler<TOptions> : OAuthHandler<TOptions> where TOptions : OAuthOptions, new()
    {
        public DaOAuthHandler(IOptionsMonitor<TOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
          : base(options, logger, encoder, clock)
        { }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic",
               Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{Options.ClientId}:{Options.ClientSecret}")));
            requestMessage.Content = requestContent;
            var response = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
            if (response.IsSuccessStatusCode)
            {
                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                var error = "OAuth token endpoint failure: " + await Display(response);
                return OAuthTokenResponse.Failed(new Exception(error));
            }
        }

        private static async Task<string> Display(HttpResponseMessage response)
        {
            var output = new StringBuilder();
            output.Append("Status: " + response.StatusCode + ";");
            output.Append("Headers: " + response.Headers.ToString() + ";");
            output.Append("Body: " + await response.Content.ReadAsStringAsync() + ";");
            return output.ToString();
        }

    }
}
