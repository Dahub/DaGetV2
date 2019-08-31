using System;
using DaGetV2.Domain.Interface;

namespace DaGetV2.Domain
{
    public class Transfert : IDomainObject
    {
        public Guid Id { get; set; }

        public Guid OperationFromId { get; set; }

        public Operation OperationFrom { get; set; }

        public Guid OperationToId { get; set; }

        public Operation OperationTo { get; set; }
    }
}
