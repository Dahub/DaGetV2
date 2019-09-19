using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Dal.EF
{
    public class CosmosDbDaGetContext : DaGetContext
    {
        public CosmosDbDaGetContext(DbContextOptions options) : base(options)
        {
        }
    }
}
