using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.DTO;
using DaGetV2.Service.Interface;
using DaGetV2.Shared.ApiTool;
using Microsoft.AspNetCore.Mvc;

namespace DaGetV2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _service;
        private readonly IContextFactory _contextFactory;

        public BankAccountController([FromServices] IContextFactory contextFactory, [FromServices] IBankAccountService bankAccountService)
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
    }
}