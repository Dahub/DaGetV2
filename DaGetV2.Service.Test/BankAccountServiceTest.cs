using System;
using System.Collections.Generic;
using System.Linq;
using DaGetV2.Dal.Interface;
using DaGetV2.Dal.Interface.Repositories;
using DaGetV2.Domain;
using DaGetV2.Service.DTO;
using DaGetV2.Shared.TestTool;
using Moq;
using Xunit;

namespace DaGetV2.Service.Test
{
    public class BankAccountServiceTest
    {
        [Fact]
        public void Add_With_Unknow_User_Should_Throw_Unauthorized_Exception()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var bankAccountType = DataBaseHelper.Instance.UseBankAccountType(dbName);
            var bankAccountService = new BankAccountService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetUnauthorizedException>(() => bankAccountService.Add(context, "not exist", new CreateBankAccountDto()
                {
                    BankAccountTypeId = bankAccountType.Id,
                    Wording = "test",
                    InitialBalance = 0m,
                    OperationsTypes = new List<string>()
                    {
                        "operation type est 1",
                        "operation type est 2",
                        "operation type est 3"
                    }
                }));
            }
        }

        [Fact]
        public void Add_Should_Add_BankAccount()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseSammyUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseBankAccountType(dbName);
            var bankAccountService = new BankAccountService();

            var bankAccountWording = "test bank account";
            var initialBalance = 120.50m;

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                bankAccountService.Add(context, user.UserName, new CreateBankAccountDto()
                {
                    BankAccountTypeId = bankAccountType.Id,
                    Wording = bankAccountWording,
                    InitialBalance = initialBalance,
                    OperationsTypes = new List<string>()
                    {
                        "operation type est 1",
                        "operation type est 2",
                        "operation type est 3"
                    }
                });
            }

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var bankAccountFromDb = context.BankAccounts.SingleOrDefault(ba => ba.Wording.Equals(bankAccountWording));

                Assert.NotNull(bankAccountFromDb);

                Assert.Equal(initialBalance, bankAccountFromDb.Balance);
                Assert.Equal(bankAccountType.Id, bankAccountFromDb.BankAccountTypeId);

                var userBankAccountFromDb = context.UserBankAccounts.SingleOrDefault(uba => uba.BankAccountId.Equals(bankAccountFromDb.Id));

                Assert.NotNull(userBankAccountFromDb);
                Assert.Equal(user.Id, userBankAccountFromDb.UserId);
                Assert.True(userBankAccountFromDb.IsOwner);
                Assert.False(userBankAccountFromDb.IsReadOnly);

                var operationsTypes = context.OperationTypes.Select(ot => ot.BankAccountId.Equals(bankAccountFromDb.Id));

                Assert.NotEmpty(operationsTypes);
                Assert.Equal(3, operationsTypes.Count());
            }
        }

        [Fact]
        public void GetAll_Should_Return_All_Bank_Account_For_User()
        {
            var bankAccountType = new BankAccountType()
            {
                Id = Guid.NewGuid(),
                Wording = "bank account type",
                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now
            };

            var user = new User()
            {
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                UserName = "Sammy",
                ModificationDate = DateTime.Now
            };

            var firstBankAccount = new BankAccount()
            {
                Balance = 50m,
                BankAccountTypeId = bankAccountType.Id,
                BankAccountType = bankAccountType,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OpeningBalance = 128.25m,
                Wording = "bank account 1"
            };

            var secondBankAccount = new BankAccount()
            {
                Balance = -90m,
                BankAccountTypeId = bankAccountType.Id,
                BankAccountType = bankAccountType,
                Id = Guid.NewGuid(),
                ModificationDate = DateTime.Now,
                OpeningBalance = 128.25m,
                Wording = "bank account 2"
            };

            bankAccountType.BanksAccounts = new List<BankAccount>()
            {
                firstBankAccount, secondBankAccount
            };

            var firstUserBankAccount = new UserBankAccount()
            {
                BankAccount = firstBankAccount,
                BankAccountId = firstBankAccount.Id,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IsOwner = true,
                IsReadOnly = false,
                ModificationDate = DateTime.Now,
                User = user,
                UserId = user.Id
            };

            var secondUserBankAccount = new UserBankAccount()
            {
                BankAccount = secondBankAccount,
                BankAccountId = secondBankAccount.Id,
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid(),
                IsOwner = false,
                IsReadOnly = false,
                ModificationDate = DateTime.Now,
                User = user,
                UserId = user.Id
            };

            user.UsersBanksAccounts = new List<UserBankAccount>()
            {
                firstUserBankAccount, secondUserBankAccount
            };

            firstBankAccount.UsersBanksAccounts = new List<UserBankAccount>()
            {
                firstUserBankAccount
            };

            secondBankAccount.UsersBanksAccounts = new List<UserBankAccount>()
            {
                secondUserBankAccount
            };

            var bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            bankAccountRepositoryMock.Setup(p => p.GetAllByUser("Sammy")).Returns(new List<BankAccount>()
            {
                firstBankAccount, secondBankAccount
            });

            var contextMock = new Mock<IContext>();
            contextMock.Setup(c => c.GetBankAccountRepository()).Returns(bankAccountRepositoryMock.Object);

            var bankAccountService = new BankAccountService();

            var bankAccounts = bankAccountService.GetAll(contextMock.Object, "Sammy");

            Assert.Equal(2, bankAccounts.Count());

            var myBankAccount = bankAccounts.SingleOrDefault(ba => ba.Id.Equals(secondBankAccount.Id.ToString()));

            Assert.NotNull(myBankAccount);

            Assert.Equal(secondBankAccount.Balance, myBankAccount.Balance);
            Assert.Equal(bankAccountType.Wording, myBankAccount.BankAccountType);
            Assert.Equal(secondUserBankAccount.IsOwner, myBankAccount.IsOwner);

            Assert.Equal(secondUserBankAccount.IsReadOnly, myBankAccount.IsReadOnly);
            Assert.Equal(secondBankAccount.Wording, myBankAccount.Wording);
        }
    }
}
