using DaGetV2.Gui.Models;
using DaGetV2.Service.DTO;
using DaGetV2.Shared.ApiTool;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            var responseContent = await response.Content.ReadAsStringAsync();

            var bankAccounts = JsonConvert.DeserializeObject<ListResult<BankAccountDto>>(responseContent);

            return View("Index", new HomeIndexModel()
            {
                BankAccountSummaries = bankAccounts.Datas.Select(ba =>
                    new BankAccountSummary()
                    {
                        Balance = ba.Balance,
                        BankAccountType = ba.BankAccountType,
                        Id = ba.Id,
                        IsOwner = ba.IsOwner,
                        IsReadOnly = ba.IsReadOnly,
                        Wording = ba.Wording
                    })
            });
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
