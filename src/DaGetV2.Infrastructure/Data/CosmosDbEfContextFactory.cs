namespace DaGetV2.Infrastructure.Data
{
    using ApplicationCore.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class CosmosDbEfContextFactory : IContextFactory
    {
        private readonly string _serviceEndPoint;
        private readonly string _key;
        private readonly string _dbName;

        private readonly DbContextOptions _options;

        public CosmosDbEfContextFactory(string serviceEndPoint, string key, string dbName)
        {
            _serviceEndPoint = serviceEndPoint;
            _key = key;
            _dbName = dbName;

            var builder = new DbContextOptionsBuilder<CosmosDbDaGetContext>();
            builder.UseCosmos<CosmosDbDaGetContext>(_serviceEndPoint, _key, _dbName);
            _options = builder.Options;
        }

        public IContext CreateContext()
        {
            return new CosmosDbDaGetContext(_options);
        }
    }
}
