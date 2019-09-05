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
    public class BankAccountTypeController : ControllerBase
    {
        private readonly IContextFactory _contextFactory;

        private readonly IBankAccountTypeService _bankAccountTypeService;

        public BankAccountTypeController([FromServices] IContextFactory contextFactory, [FromServices] IBankAccountTypeService bankAccountTypeService)
        {
            _contextFactory = contextFactory;
            _bankAccountTypeService = bankAccountTypeService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            IEnumerable<BankAccountTypeDto> bankAccountTypes;

            using (var context = _contextFactory.CreateContext())
            {
                bankAccountTypes = _bankAccountTypeService.GetAll(context);                
            }

            return Ok(bankAccountTypes.ToListResult());
        }
    }
}