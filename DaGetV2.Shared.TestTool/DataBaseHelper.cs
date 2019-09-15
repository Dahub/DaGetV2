using System;
using DaGetV2.Dal.EF;
using DaGetV2.Domain;
using Microsoft.EntityFrameworkCore;

namespace DaGetV2.Shared.TestTool
{
    public class DataBaseHelper
    {
        private static DataBaseHelper _helper;
        private static Random _rand;

        private DataBaseHelper()
        {
            _rand = new Random(DateTime.Now.Millisecond);
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

        public BankAccountType UseBankAccountType(Guid databaseName)
        {
            var bankAccountType = new BankAccountType()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                Wording = "Test bank account type"
            };

            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                               .UseInMemoryDatabase(databaseName: databaseName.ToString())
                               .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.BankAccountTypes.Add(bankAccountType);

                context.Commit();
            }

            return bankAccountType;
        }

        public Operation UseNewOperation(Guid databaseName, Guid bankAccountId)
        {
            var operation = new Operation()
            {
                Amount = GenerateNewAmount(),
                BankAccountId = bankAccountId,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OperationDate = DateTime.Now,
                OperationTypeId = Guid.NewGuid()
            };

            return AddOperation(databaseName, operation);
        }

        public Operation UseNewOperation(Guid databaseName, Guid bankAccountId, Guid operationTypeId)
        {
            var operation = new Operation()
            {
                Amount = GenerateNewAmount(),
                BankAccountId = bankAccountId,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OperationDate = DateTime.Now,
                OperationTypeId = operationTypeId
            };

            return AddOperation(databaseName, operation);
        }

        public OperationType UseNewOperationType(Guid databaseName, Guid bankAccountId)
        {
            var operationType = new OperationType()
            {
                BankAccountId = bankAccountId,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                Wording = Guid.NewGuid().ToString()
            };

            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                               .UseInMemoryDatabase(databaseName: databaseName.ToString())
                               .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.OperationTypes.Add(operationType);

                context.Commit();
            }

            return operationType;
        }

        public BankAccount UseSammyBankAccount(Guid databaseName, Guid sammyId)
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
                               .UseInMemoryDatabase(databaseName: databaseName.ToString())
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

        private static decimal GenerateNewAmount()
        {
            var amoutInt = _rand.Next(20000) - 10000;
            return (decimal)amoutInt / 100;
        }

        private static Operation AddOperation(Guid databaseName, Operation operation)
        {
            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                                           .UseInMemoryDatabase(databaseName: databaseName.ToString())
                                           .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.Operations.Add(operation);

                context.Commit();
            }

            return operation;
        }
    }
}
