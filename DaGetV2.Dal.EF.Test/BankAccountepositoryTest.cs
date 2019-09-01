using System.Linq;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class BankAccountepositoryTest : TestBase
    {       
        public BankAccountepositoryTest() : base("bankAccountTestDatabase")
        {            
        }

        [Fact]
        public void GetAllByUser_Should_Return_Banks_Accounts_With_Type_And_Users_Bank_Account()
        {
            using (var context = new DaGetContext(_dbContextOptions))
            {
                var bankAccountRepository = context.GetBankAccountRepository();

                var bankAccounts = bankAccountRepository.GetAllByUser(_sammy.UserName);

                Assert.NotNull(bankAccounts);
                Assert.NotEmpty(bankAccounts);

                var bankAccount = bankAccounts.First();

                Assert.NotNull(bankAccount.BankAccountType);
                Assert.NotNull(bankAccount.UsersBanksAccounts);
                Assert.NotEmpty(bankAccount.UsersBanksAccounts);
            }
        }
    }
}
