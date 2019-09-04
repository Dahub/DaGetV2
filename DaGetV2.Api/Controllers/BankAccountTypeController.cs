using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.Interface;
using Microsoft.AspNetCore.Http;
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
            using(var context = _contextFactory.CreateContext())
            {
                return Ok(_bankAccountTypeService.GetAll(context));
            }
        }
    }
}