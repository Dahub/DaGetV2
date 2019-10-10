namespace DaGetV2.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.Dal.Interface;
    using DaGetV2.Service.DTO;
    using DaGetV2.Service.Interface;
    using DaGetV2.Shared.ApiTool;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;


    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _service;
        private readonly IOperationTypeService _operationTypeService;
        private readonly IContextFactory _contextFactory;

        public BankAccountController(
            [FromServices] IContextFactory contextFactory, 
            [FromServices] IBankAccountService bankAccountService,
            [FromServices] IOperationTypeService operationTypeService)
        {
            _service = bankAccountService;
            _operationTypeService = operationTypeService;
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

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromHeader(Name = "username")] string userName, Guid id)
        {
            using (var context = _contextFactory.CreateContext())
            {
                _service.DeleteBankAccountById(context, userName, id);
            }

            return Ok();
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

        [HttpGet]
        [Route("{id}/operationsTypes")]
        public IActionResult GetOperationsTypesByBankAccountId([FromHeader(Name = "username")] string userName, Guid id)
        {
            IEnumerable<OperationTypeDto> operationsTypes;

            using (var context = _contextFactory.CreateContext())
            {
                operationsTypes = _operationTypeService.GetBankAccountOperationsType(context, userName, id);
            }

            return Ok(operationsTypes.ToListResult());
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