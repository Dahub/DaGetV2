namespace DaGetV2.Infrastructure.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.ApplicationCore.Interfaces;
    using Microsoft.EntityFrameworkCore;

    internal class EfRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        IContext IRepository<T>.Context { get; set; }

        public DaGetContext Context { get; set; }

        public EfRepository(DaGetContext context)
        {
            Context = context;
        }

        public void Add(T toAdd)
        {
            var now = DateTime.Now;
            toAdd.CreationDate = now;
            toAdd.ModificationDate = now;

            Context.Set<T>().Add(toAdd);
        }

        public int Count(ISpecification<T> spec)
        {
            return ApplySpecification(spec).Count();
        }

        public void Delete(T toDelete)
        {
            Context.Set<T>().Remove(toDelete);
            Context.Entry(toDelete).State = EntityState.Deleted;
        }

        public T GetById(Guid id)
        {
            return Context.Set<T>().Find(id);
        }

        public IReadOnlyList<T> ListAll()
        {
            return Context.Set<T>().ToList();
        }

        public IReadOnlyList<T> List(ISpecification<T> spec)
        {
            return ApplySpecification(spec).ToList();
        }

        public void Update(T toUpdate)
        {
            toUpdate.ModificationDate = DateTime.Now;

            Context.Set<T>().Attach(toUpdate);
            Context.Entry(toUpdate).State = EntityState.Modified;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(Context.Set<T>().AsQueryable(), spec);
        }

        public T SingleOrDefault(ISpecification<T> spec)
        {
            return ApplySpecification(spec).SingleOrDefault();
        }
    }
}
