namespace DaGetV2.Gui.Controllers
{
    using System;
    using System.Threading.Tasks;
    using ApplicationCore.DTO;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    [Authorize]
    public class OperationController : DaGetControllerBase
    {
        public OperationController(IConfiguration configuration) 
            : base(configuration)
        {
        }

        [HttpPost]
        [Route("/Operation/Close")]
        public async Task<IActionResult> CloseOperationAsync(Guid idOperation)
        {
            var operationDto = await GetToApi<OperationDto>($"operation/{idOperation}");
            operationDto.IsClosed = true;
            await PutToApi("operation", operationDto);

            return Ok();
        }
    }
}