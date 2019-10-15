namespace DaGetV2.Api.Controllers
{
    using System.Collections.Generic;
    using ApplicationCore.DTO;
    using ApplicationCore.Interfaces;
    using DaGetV2.Shared.ApiTool;
    using Infrastructure.Interfaces;
    using Microsoft.AspNetCore.Mvc;

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