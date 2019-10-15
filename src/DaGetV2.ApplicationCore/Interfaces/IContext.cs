namespace DaGetV2.ApplicationCore.Interfaces
{
    using System;

    public interface IContext : IDisposable
    {
        void Commit();

        void CommitAsync();

        IRepository<T> GetRepository<T>() where T : class, IDomainObject;
    }
}
