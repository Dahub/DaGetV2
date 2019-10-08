using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaGetV2.Gui.Models;
using DaGetV2.Service.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
            var operationTypes = await GetListToApi<OperationTypeDto>("operationtype"); 
            var bankAccountTypes = await GetListToApi<BankAccountTypeDto>("bankaccounttype");
        
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
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            var createBankAccountResponse = await PostToApi("bankAccount", new CreateBankAccountDto()
            {
                BankAccountTypeId = model.BankAccountTypeId,
                InitialBalance = model.InitialBalance,
                OperationsTypes = model.OperationTypes.Select(ot => ot.Value),
                Wording = model.Wording
            });

            return RedirectToAction("IndexAsync", "Home");
        }

        [HttpPost]
        [Route("/BankAccount/Edit")]
        public async Task<IActionResult> EditAsync(BankAccountModel model)
        {
            var response = await PutToApi("bankAccount", new UpdateBankAccountDto()
            {
                Id = model.Id,
                BankAccountTypeId = model.BankAccountTypeId,
                InitialBalance = model.InitialBalance,
                OperationsTypes = model.OperationTypes.ToList(),
                Wording = model.Wording
            });

            return RedirectToAction("IndexAsync", "Home");
        }

        [HttpGet]
        [Route("/BankAccount/Edit/{id}")]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            var operationTypes = await GetListToApi<OperationTypeDto>($"bankaccount/{id}/operationstypes");
            var bankAccountTypes = await GetListToApi<BankAccountTypeDto>("bankaccounttype");
            var bankAccount = await GetToApi<BankAccountDto>($"bankaccount/{id}");

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
            if (!Guid.TryParse(guid, out var result))
            {
                return null;
            }

            return result;
        }
    }
}