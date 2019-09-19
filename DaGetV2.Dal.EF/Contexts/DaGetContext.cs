using DaGetV2.Dal.EF.Repositories;
using DaGetV2.Dal.Interface;
using DaGetV2.Dal.Interface.Repositories;
using DaGetV2.Domain;
using DaGetV2.Shared.Constant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        public DbSet<UserBankAccount> UserBankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("daget");

            BuildBankAccount(modelBuilder);
            BuildBankAccountType(modelBuilder);
            BuildOperation(modelBuilder);
            BuildOperationType(modelBuilder);
            BuildTransfert(modelBuilder);
            BuildUser(modelBuilder);
            BuildUserBankAccount(modelBuilder);

            // remove cascade delete default behavior
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
               .SelectMany(t => t.GetForeignKeys())
               .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        private static void BuildUserBankAccount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBankAccount>().ToTable("UserBankAccount");
            modelBuilder.Entity<UserBankAccount>().HasKey(u => u.Id);
            modelBuilder.Entity<UserBankAccount>()
               .Property(ba => ba.CreationDate)
               .HasColumnName("CreationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<UserBankAccount>()
               .Property(ba => ba.ModificationDate)
               .HasColumnName("ModificationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<UserBankAccount>()
                .Property(uba => uba.IsOwner)
                .HasColumnName("IsOwner")
                .HasColumnType("bit")
                .IsRequired();
            modelBuilder.Entity<UserBankAccount>()
               .Property(uba => uba.IsReadOnly)
               .HasColumnName("IsReadOnly")
               .HasColumnType("bit")
               .IsRequired();
            modelBuilder.Entity<UserBankAccount>()
                .Property(uba => uba.UserId)
                .HasColumnName("FK_User")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            modelBuilder.Entity<UserBankAccount>()
               .HasOne<User>(uba => uba.User)
               .WithMany(u => u.UsersBanksAccounts)
               .HasForeignKey(uba => uba.UserId);
            modelBuilder.Entity<UserBankAccount>()
               .Property(uba => uba.BankAccountId)
               .HasColumnName("FK_BankAccount")
               .HasColumnType("uniqueidentifier")
               .IsRequired();
            modelBuilder.Entity<UserBankAccount>()
               .HasOne<BankAccount>(uba => uba.BankAccount)
               .WithMany(ba => ba.UsersBanksAccounts)
               .HasForeignKey(uba => uba.BankAccountId);
        }

        private static void BuildUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>()
               .Property(ba => ba.CreationDate)
               .HasColumnName("CreationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<User>()
               .Property(ba => ba.ModificationDate)
               .HasColumnName("ModificationDate")
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

        private static void BuildTransfert(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transfert>().ToTable("Transfert");
            modelBuilder.Entity<Transfert>().HasKey(t => t.Id);
            modelBuilder.Entity<Transfert>()
               .Property(ba => ba.CreationDate)
               .HasColumnName("CreationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<Transfert>()
               .Property(ba => ba.ModificationDate)
               .HasColumnName("ModificationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<Transfert>()
                .Property(t => t.OperationFromId)
                .HasColumnName("FK_OperationFrom")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            modelBuilder.Entity<Transfert>()
                .HasOne<Operation>(t => t.OperationFrom)
                .WithMany(of => of.FromTransferts)
                .HasForeignKey(t => t.OperationFromId);
            modelBuilder.Entity<Transfert>()
              .Property(t => t.OperationToId)
              .HasColumnName("FK_OperationTo")
              .HasColumnType("uniqueidentifier")
              .IsRequired();
            modelBuilder.Entity<Transfert>()
                .HasOne<Operation>(t => t.OperationTo)
                .WithMany(of => of.ToTransferts)
                .HasForeignKey(t => t.OperationToId);
        }

        private static void BuildOperationType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationType>().ToTable("OperationType");
            modelBuilder.Entity<OperationType>().HasKey(u => u.Id);
            modelBuilder.Entity<OperationType>()
               .Property(ba => ba.CreationDate)
               .HasColumnName("CreationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<OperationType>()
               .Property(ba => ba.ModificationDate)
               .HasColumnName("ModificationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<OperationType>()
               .Property(ot => ot.Wording)
               .HasColumnName("Wording")
               .HasColumnType("nvarchar(256)")
               .IsRequired();
            modelBuilder.Entity<OperationType>()
              .Property(ot => ot.BankAccountId)
              .HasColumnName("FK_BankAccount")
              .HasColumnType("uniqueidentifier")
              .IsRequired();
            modelBuilder.Entity<OperationType>()
                .HasOne<BankAccount>(ot => ot.BankAccount)
                .WithMany(ba => ba.OperationsTypes)
                .HasForeignKey(o => o.BankAccountId);
            modelBuilder.Entity<OperationType>()
               .HasMany<Operation>(ot => ot.Operations)
               .WithOne(o => o.OperationType);
        }

        private static void BuildOperation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>().ToTable("Operation");
            modelBuilder.Entity<Operation>().HasKey(ba => ba.Id);
            modelBuilder.Entity<Operation>()
                .Property(o => o.IsClosed)
                .HasColumnName("IsClosed")
                .HasColumnType("bit")
                .IsRequired();
            modelBuilder.Entity<Operation>()
               .Property(o => o.IsTransfert)
               .HasColumnName("IsTransfert")
               .HasColumnType("bit")
               .IsRequired();
            modelBuilder.Entity<Operation>()
               .Property(ba => ba.CreationDate)
               .HasColumnName("CreationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<Operation>()
               .Property(ba => ba.ModificationDate)
               .HasColumnName("ModificationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<Operation>()
                .Property(o => o.OperationDate)
                .HasColumnName("OperationDate")
                .HasColumnType("datetime")
                .IsRequired();
            modelBuilder.Entity<Operation>()
                .Property(o => o.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            modelBuilder.Entity<Operation>()
                .Property(o => o.BankAccountId)
                .HasColumnName("FK_BankAccount")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            modelBuilder.Entity<Operation>()
                .HasOne<BankAccount>(o => o.BankAccount)
                .WithMany(ba => ba.Operations)
                .HasForeignKey(o => o.BankAccountId);
            modelBuilder.Entity<Operation>()
                .Property(o => o.OperationTypeId)
                .HasColumnName("FK_OperationType")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            modelBuilder.Entity<Operation>()
                .HasOne<OperationType>(o => o.OperationType)
                .WithMany(ot => ot.Operations)
                .HasForeignKey(o => o.OperationTypeId);
            modelBuilder.Entity<Operation>()
                .HasMany<Transfert>(t => t.FromTransferts)
                .WithOne(o => o.OperationFrom);
            modelBuilder.Entity<Operation>()
                .HasMany<Transfert>(t => t.ToTransferts)
                .WithOne(o => o.OperationTo);
        }

        private static void BuildBankAccountType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccountType>().ToTable("BankAccountType");
            modelBuilder.Entity<BankAccountType>().HasKey(ba => ba.Id);
            modelBuilder.Entity<BankAccountType>()
               .Property(ba => ba.CreationDate)
               .HasColumnName("CreationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<BankAccountType>()
               .Property(ba => ba.ModificationDate)
               .HasColumnName("ModificationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<BankAccountType>()
                .Property(bat => bat.Wording)
                .HasColumnName("Wording")
                .HasColumnType("nvarchar(256)")
                .IsRequired();
            modelBuilder.Entity<BankAccountType>()
                .HasMany<BankAccount>(bat => bat.BanksAccounts)
                .WithOne(ba => ba.BankAccountType);         
        }

        private static void BuildBankAccount(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>().ToTable("BankAccount");
            modelBuilder.Entity<BankAccount>().HasKey(ba => ba.Id);
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.OpeningBalance)
                .HasColumnName("OpeningBalance")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
               .Property(ba => ba.Wording)
               .HasColumnName("Wording")
               .HasColumnType("nvarchar(256)")
               .HasMaxLength(256)
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
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
                .HasOne<BankAccountType>(ba => ba.BankAccountType)
                .WithMany(bat => bat.BanksAccounts)
                .HasForeignKey(ba => ba.BankAccountTypeId);
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.BankAccountTypeId)
                .HasColumnName("FK_BankAccountType")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
                .Property(ba => ba.CreationDate)
                .HasColumnName("CreationDate")
                .HasColumnType("datetime")
                .IsRequired();
            modelBuilder.Entity<BankAccount>()
               .Property(ba => ba.ModificationDate)
               .HasColumnName("ModificationDate")
               .HasColumnType("datetime")
               .IsRequired();
            modelBuilder.Entity<BankAccount>()
                .HasMany<OperationType>(ba => ba.OperationsTypes)
                .WithOne(ot => ot.BankAccount);
            modelBuilder.Entity<BankAccount>()
                .HasMany<UserBankAccount>(ba => ba.UsersBanksAccounts)
                .WithOne(uba => uba.BankAccount);
        }

        public void Commit()
        {
            this.SaveChanges();
        }

        public async void CommitAsync()
        {
            await SaveChangesAsync().ConfigureAwait(false);
        }

        public IUserRepository GetUserRepository() => new UserRepository() { Context = this };

        public IBankAccountRepository GetBankAccountRepository() => new BankAccountRepository() { Context = this };

        public IBankAccountTypeRepository GetBankAccountTypeRepository() => new BankAccountTypeRepository() { Context = this };

        public IOperationTypeRepository GetOperationTypeRepository() => new OperationTypeRepository() { Context = this };

        public IUserBankAccountRepository GetUserBankAccountRepository() => new UserBankAccountRepository() { Context = this };

        public IOperationRepository GetOperationRepository() => new OperationRepository() { Context = this };
    }
}
