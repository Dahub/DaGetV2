using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DaGetV2.Gui
{
    public abstract class ControllerBase : Controller
    {
        protected readonly AppConfiguration _appConfiguration;
        private readonly HttpClient _client = new HttpClient();

        public ControllerBase(IConfiguration configuration) 
        {
            _appConfiguration = configuration.GetSection("AppConfiguration").Get<AppConfiguration>();
        }

        protected async Task<HttpResponseMessage> GetToApi(string route)
        {
            return await GetToApi(route, null);
        }

        protected async Task<HttpResponseMessage> GetToApi(string route, NameValueCollection queryParams)
        {
            var myUri = BuildRouteWithParams(ref route, queryParams);

            AddAccessTokenHeader();

            return await _client.GetAsync(myUri);
        }

        protected async Task<HttpResponseMessage> PutToApi(string route, object data)
        {
            AddAccessTokenHeader();

            return await _client.PutAsJsonAsync(
                $"{_appConfiguration.DaGetApiUrl}/{route}", data);
        }
        protected async Task<HttpResponseMessage> PostToApi(string route, object data)
        {
            AddAccessTokenHeader();

            return await _client.PostAsJsonAsync(
                $"{_appConfiguration.DaGetApiUrl}/{route}", data);
        }

        protected async Task<HttpResponseMessage> DeleteToApi(string route)
        {
            AddAccessTokenHeader();

            return await _client.DeleteAsync(
               $"{_appConfiguration.DaGetApiUrl}/{route}");
        }

        protected async Task<HttpResponseMessage> HeadToApi(string route)
        {
            return await HeadToApi(route, null);
        }

        protected async Task<HttpResponseMessage> HeadToApi(string route, NameValueCollection queryParams)
        {
            var myUri = BuildRouteWithParams(ref route, queryParams);

            AddAccessTokenHeader();

            return await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Head,
                RequestUri = myUri
            });
        }

        private void AddAccessTokenHeader()
        {
            var token = User.Claims.Where(c => c.Type.Equals("access_token")).Select(c => c.Value).FirstOrDefault();
            _client.DefaultRequestHeaders.Add("access_token", token);
        }

        private Uri BuildRouteWithParams(ref string route, NameValueCollection queryParams)
        {
            if (queryParams != null)
            {
                foreach (var k in queryParams.AllKeys)
                {
                    route = String.Concat(route, $"&{k}={queryParams.GetValues(k).FirstOrDefault()}");
                }
            }

            Uri.TryCreate($"{_appConfiguration.DaGetApiUrl}/{route}", UriKind.Absolute, out var myUri);
            return myUri;
        }
    }
}
