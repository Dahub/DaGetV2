namespace DaGetV2.Api.Controllers
{
    using System;
    using ApplicationCore.DTO;
    using ApplicationCore.Interfaces;
    using Infrastructure.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly IOperationService _operationService;
        private readonly IContextFactory _contextFactory;

        public OperationController(
            [FromServices] IContextFactory contextFactory,
            [FromServices] IOperationService operationService)
        {
            _operationService = operationService;
            _contextFactory = contextFactory;
        }

        [HttpPut]
        [Route("")]
        public IActionResult Put([FromHeader(Name = "username")] string userName, UpdateOperationDto toUpdateBankAccount)
        {
            using (var context = _contextFactory.CreateContext())
            {
                _operationService.Update(context, userName, toUpdateBankAccount);
            }

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get([FromHeader(Name = "username")] string userName, Guid id)
        {
            OperationDto result;

            using (var context = _contextFactory.CreateContext())
            {
                result = _operationService.GetById(context, userName, id);
            }

            return Ok(result);
        }
    }
}