using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaGetV2.Gui.Models;
using DaGetV2.Service.DTO;
using DaGetV2.Shared.ApiTool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DaGetV2.Gui.Controllers
{
    public class BankAccountController : ControllerBase
    {
        public BankAccountController(IConfiguration configuration) 
            : base(configuration)
        {
        }

        [HttpGet]
        [Route("/BankAccount/Create")]
        public async Task<IActionResult> CreateAync()
        {
            var operationTypeResponse = await GetToApi("operationtype");
            var operationTypeResponseContent = await operationTypeResponse.Content.ReadAsStringAsync();
            var operationTypes = JsonConvert.DeserializeObject<ListResult<OperationTypeDto>>(operationTypeResponseContent);

            var bankAccountTypeResponse = await GetToApi("bankaccounttype");
            var bankAccountTypeResponseContent = await bankAccountTypeResponse.Content.ReadAsStringAsync();
            var bankAccountTypes = JsonConvert.DeserializeObject<ListResult<BankAccountTypeDto>>(bankAccountTypeResponseContent);
        
            return View(new BankAccountCreateModel()
            {
                BankAccountTypes = bankAccountTypes.Datas.ToDictionary(k => k.Id, v => v.Wording),
                OperationTypes = operationTypes.Datas.ToDictionary(k => k.Id, v => v.Wording)
            });
        }
    }
}