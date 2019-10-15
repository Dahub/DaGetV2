namespace DaGetV2.ApplicationCore.Interfaces
{
    using System;

    public interface IDomainObject
    {
        Guid Id { get; set; }

        DateTime CreationDate { get; set; }

        DateTime ModificationDate { get; set; }
    }
}
