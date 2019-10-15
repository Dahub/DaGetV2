namespace DaGetV2.Infrastructure.Data
{
    using Microsoft.EntityFrameworkCore;

    public class CosmosDbDaGetContext : DaGetContext
    {
        public CosmosDbDaGetContext(DbContextOptions options) : base(options)
        {
        }
    }
}
