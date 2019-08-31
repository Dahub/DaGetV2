using DaGetV2.Domain.Interface;
using System;
using System.Collections.Generic;

namespace DaGetV2.Domain
{
    public class Operation : IDomainObject
    {
        public Guid Id { get; set; }     
       
        public bool IsClosed { get; set; }

        public DateTime CreationDate { get; set; }

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
