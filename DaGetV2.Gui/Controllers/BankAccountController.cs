using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DaGetV2.Gui.Models;
using DaGetV2.Service.DTO;
using DaGetV2.Shared.ApiTool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DaGetV2.Gui.Controllers
{
    [Authorize]
    public class BankAccountController : ControllerBase
    {
        public BankAccountController(IConfiguration configuration) 
            : base(configuration)
        {
        }

        [HttpGet]
        [Route("/BankAccount/Create")]
        public async Task<IActionResult> CreateAsync()
        {
            var operationTypeResponse = await GetToApi("operationtype");
            if(operationTypeResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                return Unauthorized();
            }
            var operationTypeResponseContent = await operationTypeResponse.Content.ReadAsStringAsync();
            if (operationTypeResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                return Unauthorized();
            }
            var operationTypes = JsonConvert.DeserializeObject<ListResult<OperationTypeDto>>(operationTypeResponseContent);

            var bankAccountTypeResponse = await GetToApi("bankaccounttype");
            var bankAccountTypeResponseContent = await bankAccountTypeResponse.Content.ReadAsStringAsync();
            var bankAccountTypes = JsonConvert.DeserializeObject<ListResult<BankAccountTypeDto>>(bankAccountTypeResponseContent);
        
            return View("Create", new BankAccountModel()
            {
                BankAccountTypes = bankAccountTypes.Datas.ToDictionary(k => k.Id, v => v.Wording),
                OperationTypes = operationTypes.Datas.Select(ot => new KeyValuePair<Guid?, string>(ot.Id, ot.Wording))
            });
        }

        [HttpPost]
        [Route("/BankAccount/Create")]
        public async Task<IActionResult> CreateAsync(BankAccountModel model)
        {
            var createBankAccountResponse = await PostToApi("bankAccount", new CreateBankAccountDto()
            {
                BankAccountTypeId = model.BankAccountTypeId,
                InitialBalance = model.InitialBalance,
                OperationsTypes = model.OperationTypes.Select(ot => ot.Value),
                Wording = model.Wording
            });
            if (createBankAccountResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                return Unauthorized();
            }

            return View();
        }

        [HttpGet]
        [Route("/BankAccount/Edit/{id}")]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            var operationTypeResponse = await GetToApi("operationtype");
            if (operationTypeResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                return Unauthorized();
            }
            var operationTypeResponseContent = await operationTypeResponse.Content.ReadAsStringAsync();
            if (operationTypeResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                return Unauthorized();
            }
            var operationTypes = JsonConvert.DeserializeObject<ListResult<OperationTypeDto>>(operationTypeResponseContent);

            var bankAccountTypeResponse = await GetToApi("bankaccounttype");
            if (bankAccountTypeResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                return Unauthorized();
            }

            var bankAccountTypeResponseContent = await bankAccountTypeResponse.Content.ReadAsStringAsync();
            var bankAccountTypes = JsonConvert.DeserializeObject<ListResult<BankAccountTypeDto>>(bankAccountTypeResponseContent);

            var bankAccountResponse = await GetToApi($"bankaccount/{id}");
            if (bankAccountResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
            {
                return Unauthorized();
            }
            var bankAccountResponseContent = await bankAccountResponse.Content.ReadAsStringAsync();
            var bankAccount = JsonConvert.DeserializeObject<BankAccountDto>(bankAccountResponseContent);            

            return View("Edit", new BankAccountModel()
            {
                BankAccountTypeId = GuidFromString(bankAccount.BankAccountTypeId),
                Id = GuidFromString(bankAccount.Id),
                InitialBalance = bankAccount.InitialBalance,
                Wording = bankAccount.Wording,
                BankAccountTypes = bankAccountTypes.Datas.ToDictionary(k => k.Id, v => v.Wording),
                OperationTypes = operationTypes.Datas.Select(ot => new KeyValuePair<Guid?, string>(ot.Id, ot.Wording))
            });
        }

        private static Guid? GuidFromString(string guid)
        {
            Guid result;          
          
            if (!Guid.TryParse(guid, out result))
            {
                return null;
            }

            return result;
        }
    }
}