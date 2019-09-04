using System;

namespace DaGetV2.Service.DTO
{
    public class CreateBankAccountDto
    {
        public Guid BankAccountType { get; set; }

        public string Wording { get; set; }
        
        public decimal InitialBalance { get; set; }
    }
}
