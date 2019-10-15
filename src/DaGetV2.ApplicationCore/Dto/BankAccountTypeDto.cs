namespace DaGetV2.ApplicationCore.DTO
{
    using System;
    using DaGetV2.Shared.ApiTool;

    public class BankAccountTypeDto : IDto
    {
        public Guid Id { get; set; }

        public string Wording { get; set; }
    }
}
