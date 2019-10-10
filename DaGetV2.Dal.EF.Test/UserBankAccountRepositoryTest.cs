namespace DaGetV2.Dal.EF.Test
{
    using System.Linq;
    using Shared.TestTool;
    using Xunit;

    public class UserBankAccountRepositoryTest
    {
        [Fact]
        public void GetByIdUserAndIdBankAccount_Should_Return_Bank_Account()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userBankAccountRepository = context.GetUserBankAccountRepository();

                var userBankAccount = userBankAccountRepository.GetByIdUserAndIdBankAccount(user.Id, bankAccount.Id);

                Assert.NotNull(userBankAccount);
            }
        }

        [Fact]
        public void GetAllByIdBankAccount_Should_Return_All_User_Bank_Account()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user1 = DataBaseHelper.Instance.UseNewUser(dbName);
            var user2 = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user1.Id, bankAccountType.Id);
            DataBaseHelper.Instance.UseNewUserBankAccount(dbName, user2.Id, bankAccount.Id, false, false);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var userBankAccountRepository = context.GetUserBankAccountRepository();

                var usersBanksAccounts = userBankAccountRepository.GetAllByIdBankAccount(bankAccount.Id);

                Assert.NotNull(usersBanksAccounts);
                Assert.True(usersBanksAccounts.Count() == 2);
            }
        }
    }
}
