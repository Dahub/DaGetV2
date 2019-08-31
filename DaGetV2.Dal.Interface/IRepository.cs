using DaGetV2.Domain.Interface;
using System;
using System.Collections.Generic;

namespace DaGetV2.Dal.Interface
{
    public interface IRepository<T> where T : IDomainObject
    {
        IContext Context { get; set; }
        T GetById(Guid id);
        Guid Add(T toAdd);
        void Update(T toUpdate);
        void Delete(T toDelete);
        IEnumerable<T> GetAll();
    }
}
