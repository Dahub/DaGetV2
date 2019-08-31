using DaGetV2.Dal.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DaGetV2.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DaGetContext>
    {
        /// <summary>
        /// Create DB context
        /// </summary>
        /// <param name="args"></param>
        /// <returns>DB context</returns>
        public DaGetContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<DaGetContext>();
            var connectionString = configuration.GetConnectionString("DaGetConnexionString");
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("DaGetV2.Api"));
            return new DaGetContext(builder.Options);
        }
    }
}