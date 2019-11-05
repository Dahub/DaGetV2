namespace DaGetV2.ApplicationCore.DTO
{
    using System;
    using Shared.ApiTool;

    public class CreateOperationDto : IDto
    {
        public string Wording { get; set; }

        public decimal Amount { get; set; }

        public DateTime OperationDate { get; set; }

        public Guid BankAccountId { get; set; }

        public Guid OperationTypeId { get; set; }
    }
}
