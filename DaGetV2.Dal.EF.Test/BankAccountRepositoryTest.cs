namespace DaGetV2.Dal.EF.Test
{
    using System.Linq;
    using DaGetV2.Shared.TestTool;
    using Xunit;

    public class BankAccountRepositoryTest
    {     
        [Fact]
        public void GetAllByUser_Should_Return_Banks_Accounts_With_Type_And_Users_Bank_Account()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id ,bankAccountType.Id);

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

        [Fact]
        public void GetById_Should_Return_Bank_Account_With_Type_And_Users_Bank_Account()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var bankAccountRepository = context.GetBankAccountRepository();
                var bankAccountFromDb = bankAccountRepository.GetById(bankAccount.Id);

                Assert.NotNull(bankAccountFromDb);
                Assert.NotNull(bankAccountFromDb.BankAccountType);
                Assert.NotNull(bankAccountFromDb.UsersBanksAccounts);
                Assert.NotEmpty(bankAccountFromDb.UsersBanksAccounts);
                Assert.NotNull(bankAccountFromDb.UsersBanksAccounts.First().User);
            }
        }
    }
}
