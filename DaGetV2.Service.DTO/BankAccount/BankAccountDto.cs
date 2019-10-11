namespace DaGetV2.Service.DTO
{
    using System;
    using Shared.ApiTool;

    public class BankAccountDto : IDto
    {
        public Guid Id { get; set; }

        public decimal Balance { get; set; }

        public decimal InitialBalance { get; set; }

        public string Wording { get; set; }
        
        public string BankAccountTypeId { get; set; }

        public string BankAccountType { get; set; }

        public bool IsOwner { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
