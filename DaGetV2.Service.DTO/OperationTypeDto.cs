using System;
using DaGetV2.Shared.ApiTool;

namespace DaGetV2.Service.DTO
{
    public class OperationTypeDto : IDto
    {        
        public Guid Id { get; set; }

        public string Wording { get; set; }
    }
}
