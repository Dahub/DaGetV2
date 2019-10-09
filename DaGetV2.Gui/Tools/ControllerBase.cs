using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DaGetV2.Shared.ApiTool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DaGetV2.Gui
{
    public abstract class ControllerBase : Controller
    {
        protected readonly AppConfiguration _appConfiguration;
        private readonly HttpClient _client = new HttpClient();

        protected ControllerBase(IConfiguration configuration) 
        {
            _appConfiguration = configuration.GetSection("AppConfiguration").Get<AppConfiguration>();
        }

        protected async Task<T> GetToApi<T>(string route) where T : IDto
        {
            var response = await GetToApi(route);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        protected async Task<ListResult<T>> GetListToApi<T>(string route) where T : IDto
        {
            var response = await GetToApi(route);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ListResult<T>>(content);
        }

        protected async Task<HttpResponseMessage> GetToApi(string route)
        {
            return await GetToApi(route, null);            
        }

        protected async Task<HttpResponseMessage> GetToApi(string route, NameValueCollection queryParams)
        {
            var myUri = BuildRouteWithParams(ref route, queryParams);

            AddAccessTokenHeader();

            var response = await _client.GetAsync(myUri);

            ManageException(response);

            return response;
        }

        protected async Task<HttpResponseMessage> PutToApi(string route, object data)
        {
            AddAccessTokenHeader();

            var response = await _client.PutAsJsonAsync(
                $"{_appConfiguration.DaGetApiUrl}/{route}", data);

            ManageException(response);

            return response;
        }
        protected async Task<HttpResponseMessage> PostToApi(string route, object data)
        {
            AddAccessTokenHeader();

            var response = await _client.PostAsJsonAsync(
                $"{_appConfiguration.DaGetApiUrl}/{route}", data);

            ManageException(response);

            return response;
        }

        protected async Task<HttpResponseMessage> DeleteToApi(string route)
        {
            AddAccessTokenHeader();

            var response = await _client.DeleteAsync(
               $"{_appConfiguration.DaGetApiUrl}/{route}");

            ManageException(response);

            return response;
        }

        protected async Task<HttpResponseMessage> HeadToApi(string route)
        {
            return await HeadToApi(route, null);
        }

        protected async Task<HttpResponseMessage> HeadToApi(string route, NameValueCollection queryParams)
        {
            var myUri = BuildRouteWithParams(ref route, queryParams);

            AddAccessTokenHeader();

            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Head,
                RequestUri = myUri
            });

            ManageException(response);

            return response;
        }

        private static void ManageException(HttpResponseMessage response)
        {
            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                throw new UnauthorizedAccessException();
            }
        }

        private void AddAccessTokenHeader()
        {
            if(_client.DefaultRequestHeaders.Contains("access_token"))
            {
                _client.DefaultRequestHeaders.Remove("access_token");
            }
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
