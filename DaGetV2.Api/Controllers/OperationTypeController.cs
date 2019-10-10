namespace DaGetV2.Api.Controllers
{
    using DaGetV2.Dal.Interface;
    using DaGetV2.Service.Interface;
    using DaGetV2.Shared.ApiTool;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class OperationTypeController : ControllerBase
    {
        private readonly IOperationTypeService _service;
        private readonly IContextFactory _contextFactory;

        public OperationTypeController([FromServices] IContextFactory contextFactory, [FromServices] IOperationTypeService operationTypeService)
        {
            _service = operationTypeService;
            _contextFactory = contextFactory;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var operationTypes = _service.GetDefaultsOperationTypes();

            return Ok(operationTypes.ToListResult());
        }
    }
}