using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DaGetV2.Gui.Controllers
{
    public class TestController : Controller
    {
        public async Task<IActionResult> Index()
        {
            string req = "http://authapi.daoauth.fr/authorize?response_type=code&client_id=xWVvmIPVgAIYmnSs&scope=R_sc1%20RW_sc2&state=test&redirect_uri=http%3A%2F%2Flocalhost:52941/Test/Return";
            return Redirect(req);
        }

        public async Task<IActionResult> Return(string code, string error, string error_description)
        {

            var formContent = new MultipartFormDataContent()
            {
                { new StringContent(code),  "code" },
                { new StringContent("authorization_code"),  "grant_type" },
                { new StringContent("xWVvmIPVgAIYmnSs"),  "client_id" },
                { new StringContent("R_sc1 RW_sc2"),  "scope" },
                { new StringContent("http://localhost:52941/Test/Return"),  "redirect_uri" }
            };

            HttpClient cl = new HttpClient();
            cl.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            cl.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                 Convert.ToBase64String(
                      Encoding.UTF8.GetBytes("xWVvmIPVgAIYmnSs:bgoDwyJEp6FwZAK8")));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://authapi.daoauth.fr/token");

            request.Content = formContent;

            var resp = await cl.SendAsync(request);

            if ((int)resp.StatusCode >= 300)
            {
                var er = await resp.Content.ReadAsAsync<ErrorApiResultDto>();
                return View(er);
            }

            var content = await resp.Content.ReadAsAsync<Result>();

            return View(content);
        }


    }

    public class Datas
    {
        public string code { get; set; }
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string scope { get; set; }
        public string redirect_uri { get; set; }
    }

    public class Result
    {
        public string refresh_token { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
        public string scope { get; set; }

    }
    public class ErrorApiResultDto
    {
        public string Message { get; set; }
        public string Details { get; set; }
    }
}

