using System;
using DaGetV2.Domain;
using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Dal.EF.Test
{
    public abstract class TestBase : IDisposable
    {
        private readonly string _dbName;

        protected DbContextOptions _dbContextOptions;

        protected User _sammy;

        protected BankAccountType _bankAccountType;

        protected BankAccount _sammyBankAccount;

        protected UserBankAccount _sammyUserBankAccount;

        private TestBase() { }

        public TestBase(string dbName)
        {
            _dbName = dbName;
            InitDataBase();
        }

        public void Dispose()
        {
            CleanDataBase();
        }

        protected void CleanDataBase()
        {
            using (var context = new DaGetContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
            }
        }

        protected void InitDataBase()
        {
            InitEmptyDataBase();

            _sammy = new User()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                UserName = "Sammy"
            };

            _bankAccountType = new BankAccountType()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                Wording = "Test bank account type"
            };

            _sammyBankAccount = new BankAccount()
            {
                Balance = 150m,
                BankAccountTypeId = _bankAccountType.Id,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OpeningBalance = 451.25m,
                Wording = "Test bank account"
            };

            _sammyUserBankAccount = new UserBankAccount()
            {
                BankAccountId = _sammyBankAccount.Id,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IsOwner = true,
                IsReadOnly = false,
                ModificationDate = DateTime.Now,
                UserId = _sammy.Id
            };

            using (var context = new DaGetContext(_dbContextOptions))
            {
                context.Users.Add(_sammy);
                context.BankAccountTypes.Add(_bankAccountType);
                context.BankAccounts.Add(_sammyBankAccount);
                context.UserBankAccounts.Add(_sammyUserBankAccount);

                context.Commit();
            }
        }

        protected void ResetDataBase()
        {
            CleanDataBase();
            InitDataBase();
        }

        protected void InitEmptyDataBase()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                                    .UseInMemoryDatabase(databaseName: _dbName)
                                    .Options;
        }
    }
}
