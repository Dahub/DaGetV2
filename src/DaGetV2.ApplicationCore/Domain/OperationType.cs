namespace DaGetV2.ApplicationCore.Domain
{
    using DaGetV2.ApplicationCore.Interfaces;
    using System;
    using System.Collections.Generic;

    public class OperationType : IDomainObject
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public string Wording { get; set; }     
        
        public Guid BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public ICollection<Operation> Operations { get; set; }
    }
}
