using DaGetV2.Domain.Interface;
using System;
using System.Collections.Generic;

namespace DaGetV2.Domain
{
    public class BankAccountType : IDomainObject
    {
        public Guid Id { get; set; }

        public string Wording { get; set; }

        public ICollection<BankAccount> BanksAccounts { get; set; }
    }
}
