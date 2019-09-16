using System.Linq;
using DaGetV2.Shared.TestTool;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class OperationRepositoryTest
    {
        [Fact]
        public void GetAllByBankAccountId_Should_Return_All_Operations()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();

            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);

            DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);
            DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);
            DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationRepository = context.GetOperationRepository();

                var operations = operationRepository.GetAllByBankAccountId(bankAccount.Id);

                Assert.Equal(3, operations.Count());
            }
        }
    }
}
