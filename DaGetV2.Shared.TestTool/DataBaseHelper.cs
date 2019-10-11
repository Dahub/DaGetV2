namespace DaGetV2.Shared.TestTool
{
    using System;
    using System.Linq;
    using Dal.EF;
    using Domain;
    using Microsoft.EntityFrameworkCore;

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

        public User UseNewUser(Guid dataBaseName)
        {
            return UseNewUser(dataBaseName, Guid.NewGuid().ToString());
        }

        public User UseNewUser(Guid dataBaseName, string userName)
        {
            var user = new User()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                UserName = userName
            };

            return AddUser(dataBaseName, user);
        }

        public UserBankAccount UseNewUserBankAccount(
            Guid dataBaseName, 
            Guid userId, 
            Guid bankAccountId,
            bool isOwner = false, 
            bool isReadOnly = false)
        {
            var userBankAccount = new UserBankAccount()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                BankAccountId = bankAccountId,
                IsOwner = isOwner,
                IsReadOnly = isReadOnly,
                UserId = userId
            };

            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                .UseInMemoryDatabase(databaseName: dataBaseName.ToString())
                .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.UserBankAccounts.Add(userBankAccount);

                context.Commit();
            }

            return userBankAccount;
        }

        public BankAccountType UseNewBankAccountType(Guid databaseName)
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

        public Operation UseNewOperation(Guid databaseName, Guid bankAccountId, DateTime operationDate)
        {
            var operation = new Operation()
            {
                Amount = GenerateNewAmount(),
                BankAccountId = bankAccountId,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OperationDate = operationDate,
                OperationTypeId = Guid.NewGuid()
            };

            return AddOperation(databaseName, operation);
        }

        public Operation UseNewOperation(Guid databaseName, Guid bankAccountId, Guid operationTypeId, DateTime operationDate)
        {
            var operation = new Operation()
            {
                Amount = GenerateNewAmount(),
                BankAccountId = bankAccountId,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OperationDate = operationDate,
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

        public BankAccount UseNewBankAccount(Guid databaseName, Guid userId, Guid bankAccountTypeId)
        {
            var bankAccountAmount = GenerateNewAmount();

            var bankAccount = new BankAccount()
            {
                Balance = bankAccountAmount,
                BankAccountTypeId = bankAccountTypeId,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OpeningBalance = bankAccountAmount,
                Wording = "Test bank account"
            };

            var userBankAccount = new UserBankAccount()
            {
                BankAccountId = bankAccount.Id,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IsOwner = true,
                IsReadOnly = false,
                ModificationDate = DateTime.Now,
                UserId = userId
            };

            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                               .UseInMemoryDatabase(databaseName: databaseName.ToString())
                               .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.BankAccounts.Add(bankAccount);
                context.UserBankAccounts.Add(userBankAccount);

                context.Commit();
            }

            return bankAccount;
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

                var bankAccount = context.BankAccounts.Single(ba => ba.Id.Equals(operation.BankAccountId));
                bankAccount.Balance += operation.Amount;
                context.BankAccounts.Update(bankAccount);

                context.Commit();
            }

            return operation;
        }

        private static User AddUser(Guid dataBaseName, User user)
        {
            var dbContextOptions = new DbContextOptionsBuilder<DaGetContext>()
                                            .UseInMemoryDatabase(databaseName: dataBaseName.ToString())
                                            .Options;

            using (var context = new DaGetContext(dbContextOptions))
            {
                context.Users.Add(user);

                context.Commit();
            }

            return user;
        }

    }
}
