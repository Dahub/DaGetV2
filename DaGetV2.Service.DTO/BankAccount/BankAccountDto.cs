using DaGetV2.Shared.ApiTool;

namespace DaGetV2.Service.DTO
{
    public class BankAccountDto : IDto
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public decimal InitialBalance { get; set; }

        public string Wording { get; set; }
        
        public string BankAccountTypeId { get; set; }

        public string BankAccountType { get; set; }

        public bool IsOwner { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
