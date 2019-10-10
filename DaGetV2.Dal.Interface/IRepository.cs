namespace DaGetV2.Dal.Interface
{
    using DaGetV2.Domain.Interface;
    using System;
    using System.Collections.Generic;

    public interface IRepository<T> where T : IDomainObject
    {
        IContext Context { get; set; }

        T GetById(Guid id);

        void Add(T toAdd);

        void Update(T toUpdate);

        void Delete(T toDelete);

        IEnumerable<T> GetAll();
    }
}
