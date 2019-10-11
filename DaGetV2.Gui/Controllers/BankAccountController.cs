namespace DaGetV2.Gui.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Models;
    using Service.DTO;

    [Authorize]
    public class BankAccountController : DaGetControllerBase
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

            if (!await model.ValidateAsync(createBankAccountResponse))
            {
                return View("Create", model);
            }

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

        [HttpGet]
        [Route("/BankAccount/Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await DeleteToApi($"bankAccount/{id}");

            return RedirectToAction("IndexAsync", "Home");
        }

        [HttpPost]
        [Route("/BankAccount/Edit")]
        public async Task<IActionResult> EditAsync(BankAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }

            var updateBankAccountResponse = await PutToApi("bankAccount", new UpdateBankAccountDto()
            {
                Id = model.Id,
                BankAccountTypeId = model.BankAccountTypeId,
                InitialBalance = model.InitialBalance,
                OperationsTypes = model.OperationTypes.ToList(),
                Wording = model.Wording
            });

            if (!await model.ValidateAsync(updateBankAccountResponse))
            {
                return View("Edit", model);
            }

            return RedirectToAction("IndexAsync", "Home");
        }

        [HttpGet]
        [Route("/BankAccount/Detail/{id}")]
        public async Task<IActionResult> DetailAsync(Guid id)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;

            var startDate = new DateTime(year, month, 1).Date.ToString("yyyyMMdd");
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).Date.ToString("yyyyMMdd");

            var bankAccount = await GetToApi<BankAccountDto>($"bankaccount/{id}");
            var operations = await GetListToApi<OperationDto>($"bankaccount/{id}/operations/{startDate}/{endDate}");

            return View("Detail");
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