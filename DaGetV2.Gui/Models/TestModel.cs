using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DaGetV2.Gui.Models
{
    public class TestModel
    {
        public async Task<HttpResponseMessage> AskAuthorizeAsync()
        {
            string req = "http://authapi.daoauth.fr/authorize?response_type=code&client_id=aHC1eY8wrdXecFWc&state=test&redirect_uri=https%3A%2F%2Flocalhost:44359/Test/Return";

            HttpClient c = new HttpClient();
            var response = await c.GetAsync(req);

            return response;
        }
    }
}
