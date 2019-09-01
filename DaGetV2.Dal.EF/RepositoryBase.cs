using DaGetV2.Dal.Interface;
using DaGetV2.Domain.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DaGetV2.Dal.EF
{
    internal abstract class RepositoryBase<T> : IRepository<T> where T : class, IDomainObject
    {
        IContext IRepository<T>.Context { get; set; }

        public DaGetContext Context { get; set; }

        public virtual Guid Add(T toAdd)
        {
            var now = DateTime.Now;
            toAdd.CreationDate = now;
            toAdd.ModificationDate = now;
            
            Context.Set<T>().Add(toAdd);
            return toAdd.Id;
        }

        public virtual void Delete(T toDelete)
        {
            Context.Set<T>().Remove(toDelete);
            Context.Entry(toDelete).State = EntityState.Deleted;
        }

        public virtual T GetById(Guid id)
        {
            return Context.Set<T>().
               Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        public virtual void Update(T toUpdate)
        {
            toUpdate.ModificationDate = DateTime.Now;

            Context.Set<T>().Attach(toUpdate);
            Context.Entry(toUpdate).State = EntityState.Modified;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
    }
}
