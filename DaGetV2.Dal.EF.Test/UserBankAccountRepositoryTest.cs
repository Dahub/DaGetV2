using System.Linq;
using DaGetV2.Shared.TestTool;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class UserBankAccountRepositoryTest
    {
        [Fact]
        public void GetByIdUserAndIdBankAccount_Should_Return_Bank_Account()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseSammyUser(dbName);
            var bankAccount = DataBaseHelper.Instance.UseSammyBankAccount(dbName, user.Id);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userBankAccountRepository = context.GetUserBankAccountRepository();

                var userBankAccount = userBankAccountRepository.GetByIdUserAndIdBankAccount(user.Id, bankAccount.Id);

                Assert.NotNull(userBankAccount);
            }
        }
    }
}
