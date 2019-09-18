using System;
using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.DTO;
using DaGetV2.Service.Interface;
using DaGetV2.Shared.ApiTool;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DaGetV2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _service;
        private readonly IContextFactory _contextFactory;

        public BankAccountController(
            [FromServices] IContextFactory contextFactory, 
            [FromServices] IBankAccountService bankAccountService)
        {
            _service = bankAccountService;
            _contextFactory = contextFactory;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get([FromHeader(Name = "username")] string userName)
        {
            IEnumerable<BankAccountDto> bankAccounts;

            using (var context = _contextFactory.CreateContext())
            {
                bankAccounts = _service.GetAll(context, userName);
            }

            return Ok(bankAccounts.ToListResult());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromHeader(Name = "username")] string userName, Guid id)
        {
            BankAccountDto bankAccount;

            using (var context = _contextFactory.CreateContext())
            {
                bankAccount = _service.GetById(context, userName, id);
            }

            return Ok(bankAccount);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromHeader(Name = "username")] string userName, CreateBankAccountDto toCreateBankAccount)
        {
            Guid createdBankAccountid;

            using (var context = _contextFactory.CreateContext())
            {
                createdBankAccountid = _service.Create(context, userName, toCreateBankAccount);
            }

            var currentUrl = UriHelper.GetDisplayUrl(Request);
            return Created($"{currentUrl}/{createdBankAccountid}", null);
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put([FromHeader(Name = "username")] string userName, UpdateBankAccountDto toUpdateBankAccount)
        {
            using (var context = _contextFactory.CreateContext())
            {
                _service.Update(context, userName, toUpdateBankAccount);
            }

            return Ok();
        }
    }
}