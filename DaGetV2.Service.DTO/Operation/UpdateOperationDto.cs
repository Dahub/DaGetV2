namespace DaGetV2.Service.DTO
{
    using System;
    using Shared.ApiTool;

    public class UpdateOperationDto : IDto
    {
        public Guid Id { get; set; }

        public Guid OperationTypeId { get; set; }

        public string Wording { get; set; }

        public bool IsClosed { get; set; }

        public DateTime OperationDate { get; set; }

        public decimal Amount { get; set; }
    }
}
