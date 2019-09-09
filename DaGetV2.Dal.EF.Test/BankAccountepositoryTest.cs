using System.Linq;
using DaGetV2.Shared.TestTool;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class BankAccountepositoryTest
    {     
        [Fact]
        public void GetAllByUser_Should_Return_Banks_Accounts_With_Type_And_Users_Bank_Account()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseSammyUser(dbName);
            DataBaseHelper.Instance.UserSammyBankAccount(dbName, user.Id);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var bankAccountRepository = context.GetBankAccountRepository();

                var bankAccounts = bankAccountRepository.GetAllByUser(user.UserName);

                Assert.NotNull(bankAccounts);
                Assert.NotEmpty(bankAccounts);

                var bankAccount = bankAccounts.First();

                Assert.NotNull(bankAccount.BankAccountType);
                Assert.NotNull(bankAccount.UsersBanksAccounts);
                Assert.NotEmpty(bankAccount.UsersBanksAccounts);
                Assert.NotNull(bankAccount.UsersBanksAccounts.First().User);
            }
        }
    }
}
