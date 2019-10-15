namespace DaGetV2.ApplicationCore.DTO
{
    using System;
    using Shared.ApiTool;

    public class OperationDto : IDto
    {
        public Guid Id { get; set; }

        public string Wording { get; set; }

        public bool IsClosed { get; set; }

        public DateTime OperationDate { get; set; }

        public decimal Amount { get; set; }

        public bool IsTransfert { get; set; }

        public Guid BankAccountId { get; set; }

        public Guid OperationTypeId { get; set; }

        public string OperationTypeWording { get; set; }
    }
}
