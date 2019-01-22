using DaGetV2.Domain.Interface;

namespace DaGetV2.Domain
{
    public class Transfert : IDomainObject
    {
        public int Id { get; set; }
        public int OperationFromId { get; set; }
        public Operation OperationFrom { get; set; }
        public int OperationToId { get; set; }
        public Operation OperationTo { get; set; }
    }
}
