namespace DaGetV2.Domain
{
    using DaGetV2.Domain.Interface;
    using System;
    using System.Collections.Generic;

    public class BankAccountType : IDomainObject
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public string Wording { get; set; }

        public ICollection<BankAccount> BanksAccounts { get; set; }
    }
}
