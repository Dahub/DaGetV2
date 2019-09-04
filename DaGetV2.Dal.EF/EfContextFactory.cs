using DaGetV2.Dal.Interface;
using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Dal.EF
{
    public class EfContextFactory : IContextFactory
    {
        private readonly string _connexionString;

        private readonly DbContextOptions _options;

        public EfContextFactory(string connexionString)
        {
            _connexionString = connexionString;
            var builder = new DbContextOptionsBuilder<DaGetContext>();
            builder.UseSqlServer(_connexionString, b => b.MigrationsAssembly("DaGetV2.Dal.EF"));
            _options = builder.Options;
        }

        public IContext CreateContext()
        {
            return new DaGetContext(_options);
        }
    }
}
