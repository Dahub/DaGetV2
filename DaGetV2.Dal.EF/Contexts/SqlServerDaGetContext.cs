namespace DaGetV2.Dal.EF
{
    using System;
    using DaGetV2.Domain;
    using DaGetV2.Shared.Constant;
    using Microsoft.EntityFrameworkCore;

    public class SqlServerDaGetContext : DaGetContext
    {
        public SqlServerDaGetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccountType>()
                .HasData(new BankAccountType()
                {
                    Id = BankAccountTypeIds.Current,
                    Wording = "Courant",
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now
                }, new BankAccountType()
                {
                    Id = BankAccountTypeIds.Saving,
                    Wording = "Epargne",
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now
                });
        }
    }
}
