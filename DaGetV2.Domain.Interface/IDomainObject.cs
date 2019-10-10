namespace DaGetV2.Domain.Interface
{
    using System;

    public interface IDomainObject
    {
        Guid Id { get; set; }

        DateTime CreationDate { get; set; }

        DateTime ModificationDate { get; set; }
    }
}
