using DaGetV2.Dal.Interface;
using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Dal.EF
{
    public class EfContextFactory : IContextFactory
    {
        public string ConnexionString { get; set; }

        public IContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<DaGetContext>();
            builder.UseSqlServer(ConnexionString, b => b.MigrationsAssembly("DaGetV2.Dal.EF"));

            return new DaGetContext(builder.Options);
        }
    }
}
