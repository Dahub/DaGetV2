using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Dal.Interface.Repositories;
using DaGetV2.Domain;
using Moq;
using Xunit;

namespace DaGetV2.Service.Test
{
    public class BankAccountServiceTest
    {
        public BankAccountServiceTest()
        {            
        }

        [Fact]
        public void GetAll_Should_Return_All_Bank_Account_For_User()
        {
            var bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            bankAccountRepositoryMock.Setup(p => p.GetAllByUser("Sammy")).Returns(new List<BankAccount>());

            var contextMock = new Mock<IContext>();
            contextMock.Setup(c => c.GetBankAccountRepository()).Returns(bankAccountRepositoryMock.Object);

            var bankAccountService = new BankAccountService();

            bankAccountService.GetAll(contextMock.Object, "Sammy");
        }
    }
}
