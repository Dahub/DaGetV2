using System.Collections.Generic;
using System.Linq;
using DaGetV2.Domain;
using DaGetV2.Service.DTO;

namespace DaGetV2.Service
{
    public static class ExtensionMethod
    {
        public static IEnumerable<BankAccountDto> ToDto(this IEnumerable<BankAccount> bankAccounts, string userName)
        {
            return bankAccounts.Select(ba => ba.ToDto(userName));
        }

        public static BankAccountDto ToDto(this BankAccount bankAccount, string userName)
        {
            var isOwner = bankAccount.UsersBanksAccounts.FirstOrDefault(uba => uba.User.UserName.Equals(userName)).IsOwner;

            return new BankAccountDto()
            {
                Id = bankAccount.Id,
                Balance = bankAccount.Balance,
                BankAccountType = bankAccount.BankAccountType.Wording,
                IsOwner = isOwner,
                IsReadOnly = !isOwner,
                Wording = bankAccount.Wording
            };
        }
    }
}
