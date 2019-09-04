using System;
using System.Collections.Generic;
using DaGetV2.Domain;
using Xunit;

namespace DaGetV2.Service.Test
{
    public class ExtensionMethodTest
    {
        [Fact]
        public void ToDto_Should_Return_BankAccountDto_When_BankAccount_And_User_Are_Not_Null()
        {
            var userName = "Sammy";
            var bankAccountId = Guid.NewGuid();
            var bankAccountTypeId = Guid.NewGuid();
            var now = DateTime.Now;

            var bankAccount = new BankAccount()
            {
                Balance = 148.36m,
                BankAccountType = new BankAccountType()
                {
                    CreationDate = now,
                    Id = bankAccountTypeId,
                    ModificationDate = now,
                    Wording = "bank account type"
                },
                BankAccountTypeId = bankAccountTypeId,
                CreationDate = now,
                Id = bankAccountId,
                ModificationDate = now,
                OpeningBalance = 1200.00m,
                UsersBanksAccounts = new List<UserBankAccount>()
                {
                    new UserBankAccount()
                    {
                        BankAccountId = bankAccountId,
                        CreationDate = now,
                        Id = Guid.NewGuid(),
                        IsOwner = true,
                        IsReadOnly = false,
                        ModificationDate = now,
                        UserId = Guid.NewGuid(),
                        User = new User()
                        {
                            CreationDate = now,
                            Id = Guid.NewGuid(),
                            ModificationDate = now,
                            UserName = userName
                        }
                    }
                },
                Wording = "bank account"                
            };

            var bankAccountDto = bankAccount.ToDto(userName);

            Assert.Equal(148.36m, bankAccountDto.Balance);
            Assert.Equal("bank account type", bankAccountDto.BankAccountType);
            Assert.Equal(bankAccountId.ToString(), bankAccountDto.Id.ToString());
            Assert.True(bankAccountDto.IsOwner);
            Assert.False(bankAccountDto.IsReadOnly);
            Assert.Equal("bank account", bankAccountDto.Wording);
        }
    }
}
