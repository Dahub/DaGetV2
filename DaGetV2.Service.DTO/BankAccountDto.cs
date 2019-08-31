using System;

namespace DaGetV2.Service.DTO
{
    public class BankAccountDto
    {
        public Guid Id { get; set; }

        public decimal Balance { get; set; }
        
        public string Wording { get; set; }

        public string BankAccountType { get; set; }

        public bool IsOwner { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
