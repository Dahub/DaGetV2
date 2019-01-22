using DaGetV2.Dal.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using DaGetV2.Domain;

namespace DaGetV2.Dal.EF
{
    public class DaGetContext : DbContext, IContext
    {
        public DaGetContext(DbContextOptions options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationType> OperationTypes { get; set; }
        public DbSet<Transfert> Transferts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBankAccount> UserBankAccount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("daget");

            modelBuilder.Entity<BankAccount>().ToTable("BankAccount");
            modelBuilder.Entity<BankAccount>().HasKey(ba => ba.Id);
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.OpeningBalance)
                .HasColumnName("OpeningBalance")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
                .HasMany<Operation>(ba => ba.Operations)
                .WithOne(o => o.BankAccount);
            modelBuilder.Entity<BankAccount>()
                .HasMany(ba => ba.OperationsTypes)
                .WithOne(ot => ot.BankAccount);
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.Balance)
                .HasColumnName("Balance")
                .HasColumnType("decimal(18,2")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
                .HasOne<BankAccountType>(ba => ba.BankAccountType)
                .WithMany(bat => bat.BanksAccounts)
                .HasForeignKey(ba => ba.BankAccountTypeId);
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.BankAccountTypeId)
                .HasColumnName("FK_BankAccountType")
                .HasColumnType("integer")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.CreationDate)
                .HasColumnName("CreationDate")
                .HasColumnType("datetime")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
                .HasMany<OperationType>(ba => ba.OperationsTypes)
                .WithOne(ot => ot.BankAccount);
            modelBuilder.Entity<BankAccount>()
                .HasMany<UserBankAccount>(ba => ba.UsersBanksAccounts)
                .WithOne(uba => uba.BankAccount);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>()
                .Property(u => u.LastConnexionDate)
                .HasColumnName("LastConnexionDate")
                .HasColumnType("datetime")
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(u => u.UserName)                
                .HasColumnName("UserName")
                .HasColumnType("nvarchar(256)")
                .HasMaxLength(256)
                .IsRequired();
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        public async void CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
