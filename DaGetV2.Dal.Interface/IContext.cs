using System;

namespace DaGetV2.Dal.Interface
{
    public interface IContext : IDisposable
    {
        void Commit();
        void CommitAsync();
    }
}
