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

            var user = DataBaseHelper.Instance.UseSammyUser(dbName);
            var bankAccount = DataBaseHelper.Instance.UseSammyBankAccount(dbName, user.Id);

            DataBaseHelper.Instance.GenerateNewOperation(dbName, bankAccount.Id);
            DataBaseHelper.Instance.GenerateNewOperation(dbName, bankAccount.Id);
            DataBaseHelper.Instance.GenerateNewOperation(dbName, bankAccount.Id);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationRepository = context.GetOperationRepository();

                var operations = operationRepository.GetAllByBankAccountId(bankAccount.Id);

                Assert.Equal(3, operations.Count());
            }
        }
    }
}
