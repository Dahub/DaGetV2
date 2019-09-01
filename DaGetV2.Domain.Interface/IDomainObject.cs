using System;

namespace DaGetV2.Domain.Interface
{
    public interface IDomainObject
    {
        Guid Id { get; set; }

        DateTime CreationDate { get; set; }

        DateTime ModificationDate { get; set; }
    }
}
