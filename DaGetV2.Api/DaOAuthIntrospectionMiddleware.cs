using DaGetV2.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DaGetV2.Api
{
    public class DaOAuthIntrospectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppConfiguration _conf;

        public string DaAOuthIntrospectionUrl { get; set; }

        public DaOAuthIntrospectionMiddleware(RequestDelegate next, IConfiguration Configuration)
        {
            _next = next;
            _conf = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            StringValues token = string.Empty;
            if (!context.Request.Headers.TryGetValue("access_token", out token))
                await ExitWithUnauthorize(context);

            // challenge access_token
            var formContent = new MultipartFormDataContent()
            {
                { new StringContent(token),  "token" }
            };

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                 Convert.ToBase64String(
                     Encoding.UTF8.GetBytes($"{_conf.RessourceServerName}:{_conf.Password}")));

            var request = new HttpRequestMessage(HttpMethod.Post, _conf.IntrospectionEndPoint)
            {
                Content = formContent
            };

            var response = await httpClient.SendAsync(request);

            if ((int)response.StatusCode >= 300)
                await ExitWithUnauthorize(context);

            var result = await response.Content.ReadAsAsync<Result>();

            if (!result.active)
                await ExitWithUnauthorize(context);

            context.Request.Headers.Add("username", result.name);

            await _next(context);
        }

        private static async Task ExitWithUnauthorize(HttpContext context)
        {
            await context.Response.WriteAsync("access_token header missing or invalid token");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await Task.CompletedTask;
        }

        private class Result
        {
            public bool active { get; set; }

            public long exp { get; set; }

            public string[] aud { get; set; }

            public string client_id { get; set; }

            public string name { get; set; }
            
            public string scope { get; set; }
        }
    }
}
