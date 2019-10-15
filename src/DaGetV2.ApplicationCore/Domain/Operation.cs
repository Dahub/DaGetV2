namespace DaGetV2.ApplicationCore.Domain
{
    using DaGetV2.ApplicationCore.Interfaces;
    using System;
    using System.Collections.Generic;

    public class Operation : IDomainObject
    {
        public Guid Id { get; set; }

        public string Wording { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public bool IsClosed { get; set; }

        public DateTime OperationDate { get; set; }

        public decimal Amount { get; set; }

        public bool IsTransfert { get; set; }

        public Guid BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }
       
        public Guid OperationTypeId { get; set; }

        public OperationType OperationType { get; set; }

        public ICollection<Transfert> FromTransferts { get; set; }

        public ICollection<Transfert> ToTransferts { get; set; }
    }
}
