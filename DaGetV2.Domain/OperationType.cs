using DaGetV2.Domain.Interface;
using System.Collections.Generic;

namespace DaGetV2.Domain
{
    public class OperationType : IDomainObject
    {
        public int Id { get; set; }
        public string Wording { get; set; }        
        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
        public ICollection<Operation> Operations { get; set; }
    }
}
