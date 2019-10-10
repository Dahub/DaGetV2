namespace DaGetV2.Service.DTO
{
    using DaGetV2.Shared.ApiTool;

    public class ApiErrorResultDto : IDto
    {
        public string Message { get; set; }

        public string Details { get; set; }
    }
}

