using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DaGetV2.Dal.Interface;
using DaGetV2.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace DaGetV2.Api
{
    public class DaOAuthIntrospectionMiddleware
    {
        private static readonly ConcurrentDictionary<string, bool>  _persistedUsers = new ConcurrentDictionary<string, bool>();

        private readonly RequestDelegate _next;
        private readonly IContextFactory _contextFactory;
        private readonly AppConfiguration _conf;

        public string DaAOuthIntrospectionUrl { get; set; }

        public DaOAuthIntrospectionMiddleware(RequestDelegate next, IConfiguration Configuration, IContextFactory contextFactory)
        {
            _next = next;
            _contextFactory = contextFactory;
            _conf = Configuration.GetSection("AppConfiguration").Get<AppConfiguration>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            StringValues token = string.Empty;
            if (!context.Request.Headers.TryGetValue("access_token", out token))
            {
                await ExitWithUnauthorize(context);
            }

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
            {
                await ExitWithUnauthorize(context);
            }

            var responseContent = await response.Content.ReadAsAsync<Result>();

            if (!responseContent.active)
            {
                await ExitWithUnauthorize(context);
            }

            context.Request.Headers.Add("username", responseContent.name);

            PersistUserIfNeeded(responseContent);

            await _next(context);
        }

        private void PersistUserIfNeeded(Result responseContent)
        {
            if (!_persistedUsers.ContainsKey(responseContent.name) || !_persistedUsers.TryGetValue(responseContent.name, out var userPersisted) || !userPersisted)
            {
                using (var dbContext = _contextFactory.CreateContext())
                {
                    try
                    {
                        var userRepository = dbContext.GetUserRepository();

                        if (!userRepository.UserExists(responseContent.name))
                        {
                            userRepository.Add(new Domain.User()
                            {
                                Id = Guid.NewGuid(),
                                CreationDate = DateTime.Now,
                                UserName = responseContent.name
                            });

                            dbContext.Commit();
                        }
                    }
                    catch(Exception ex)
                    {
                        var tt = ex;
                    }
                }
                _persistedUsers.TryAdd(responseContent.name, true);
            }
        }

        private static async Task ExitWithUnauthorize(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("access_token header missing or invalid token");
            
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
