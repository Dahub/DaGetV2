using System;
using DaGetV2.Dal.EF;
using DaGetV2.Domain;
using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Shared.TestTool
{
    public class DataBaseHelper
    {
        private static DataBaseHelper _helper;

        private DataBaseHelper()
        {
        }

        public static DataBaseHelper Instance
        {
            get
            {
                if(_helper == null)
                {
                    _helper = new DataBaseHelper();
                }
                return _helper;
            }
        }

        public Guid NewDataBase()
        {
            var dataBaseName = Guid.NewGuid();

            CleanDataBase(dataBaseName);

            return dataBaseName;
        }

        public void CleanDataBase(Guid dataBaseName)
        {
            try
            {
                var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                                      .UseInMemoryDatabase(databaseName: dataBaseName.ToString())
                                      .Options;
                using (var context = new DaGetContext(dbContextOptions))
                {
                    context.Database.EnsureDeleted();
                }
            }
            catch { }
        }

        public DaGetContext CreateContext(Guid dataBaseName)
        {
            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                                .UseInMemoryDatabase(databaseName: dataBaseName.ToString())
                                .Options;

            return new DaGetContext(dbContextOptions);
        }

        public User UseSammyUser(Guid dataBaseName)
        {
            var sammy = new User()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                UserName = "Sammy"
            };

            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                                .UseInMemoryDatabase(databaseName: dataBaseName.ToString())
                                .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.Users.Add(sammy);

                context.Commit();
            }

            return sammy;
        }

        public BankAccountType UseBankAccountType(Guid dataBaseName)
        {
            var bankAccountType = new BankAccountType()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                Wording = "Test bank account type"
            };

            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                               .UseInMemoryDatabase(databaseName: dataBaseName.ToString())
                               .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.BankAccountTypes.Add(bankAccountType);

                context.Commit();
            }

            return bankAccountType;
        }

        public BankAccount UseSammyBankAccount(Guid dataBaseName, Guid sammyId)
        {
            var bankAccountType = new BankAccountType()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                Wording = "Test bank account type"
            };

            var sammyBankAccount = new BankAccount()
            {
                Balance = 150m,
                BankAccountTypeId = bankAccountType.Id,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OpeningBalance = 451.25m,
                Wording = "Test bank account"
            };

            var sammyUserBankAccount = new UserBankAccount()
            {
                BankAccountId = sammyBankAccount.Id,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IsOwner = true,
                IsReadOnly = false,
                ModificationDate = DateTime.Now,
                UserId = sammyId
            };

            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                               .UseInMemoryDatabase(databaseName: dataBaseName.ToString())
                               .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.BankAccountTypes.Add(bankAccountType);
                context.BankAccounts.Add(sammyBankAccount);
                context.UserBankAccounts.Add(sammyUserBankAccount);

                context.Commit();
            }

            return sammyBankAccount;
        }
    }
}
