using DaGetV2.Shared.ApiTool;

namespace DaGetV2.Service.DTO
{
    public class ApiErrorResultDto : IDto
    {
        public string Message { get; set; }

        public string Details { get; set; }
    }
}

