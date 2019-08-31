using DaGetV2.Gui.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DaGetV2.Gui.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        public HomeController(IConfiguration configuration) : base(configuration)
        {

        }

        [HttpGet]
        [Route("/Home/Index")]
        public async Task<IActionResult> IndexAsync()
        {
            var response = await GetToApi("bankaccount");

            return View("Index");
        }

        [HttpGet]
        [Route("/Home/Logout")]
        public async Task<RedirectResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect(Url.Content(_appConfiguration.LogoutUrl));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
