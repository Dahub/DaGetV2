using DaGetV2.Domain.Interface;

namespace DaGetV2.Domain
{
    public class UserBankAccount : IDomainObject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public bool IsOwner { get; set; }
    }
}
