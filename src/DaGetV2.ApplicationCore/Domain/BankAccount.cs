namespace DaGetV2.ApplicationCore.Domain
{
    using DaGetV2.ApplicationCore.Interfaces;
    using System;
    using System.Collections.Generic;

    public class BankAccount : IDomainObject
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public Guid BankAccountTypeId { get; set; }

        public BankAccountType BankAccountType { get; set; }

        public decimal Balance { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal ActualBalance { get; set; }

        public string Wording { get; set; }

        public ICollection<UserBankAccount> UsersBanksAccounts { get; set; }

        public ICollection<Operation> Operations { get; set; }

        public ICollection<OperationType> OperationsTypes { get; set; }
    }
}
