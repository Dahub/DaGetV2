namespace DaGetV2.ApplicationCore.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IRepository<T> where T : class, IDomainObject
    {
        IContext Context { get; set; }

        T GetById(Guid id);

        void Add(T toAdd);

        void Update(T toUpdate);

        void Delete(T toDelete);

        IReadOnlyList<T> ListAll();

        IReadOnlyList<T> List(ISpecification<T> spec);

        int Count(ISpecification<T> spec);

        T SingleOrDefault(ISpecification<T> spec);
    }
}
