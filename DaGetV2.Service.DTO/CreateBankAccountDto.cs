using System;
using DaGetV2.Shared.ApiTool;

namespace DaGetV2.Service.DTO
{
    public class CreateBankAccountDto : IDto
    {
        public Guid BankAccountType { get; set; }

        public string Wording { get; set; }
        
        public decimal InitialBalance { get; set; }
    }
}
