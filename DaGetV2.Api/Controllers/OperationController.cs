namespace DaGetV2.Api.Controllers
{
    using Dal.Interface;
    using Microsoft.AspNetCore.Mvc;
    using Service.DTO;
    using Service.Interface;

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
    }
}