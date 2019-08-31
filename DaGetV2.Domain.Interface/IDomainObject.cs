using System;

namespace DaGetV2.Domain.Interface
{
    public interface IDomainObject
    {
        Guid Id { get; set; }
    }
}
