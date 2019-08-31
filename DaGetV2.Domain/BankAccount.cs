using DaGetV2.Domain.Interface;
using System;
using System.Collections.Generic;

namespace DaGetV2.Domain
{
    public class BankAccount : IDomainObject
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid BankAccountTypeId { get; set; }

        public BankAccountType BankAccountType { get; set; }

        public decimal Balance { get; set; }

        public decimal OpeningBalance { get; set; }

        public string Wording { get; set; }

        public ICollection<UserBankAccount> UsersBanksAccounts { get; set; }

        public ICollection<Operation> Operations { get; set; }

        public ICollection<OperationType> OperationsTypes { get; set; }
    }
}
