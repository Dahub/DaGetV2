namespace DaGetV2.Dal.EF
{
    using Microsoft.EntityFrameworkCore;

    public class CosmosDbDaGetContext : DaGetContext
    {
        public CosmosDbDaGetContext(DbContextOptions options) : base(options)
        {
        }
    }
}
