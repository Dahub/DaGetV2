using System.Linq;
using DaGetV2.Shared.TestTool;
using Xunit;

namespace DaGetV2.Dal.EF.Test
{
    public class OperationTypeRepositoryTest
    {
        [Fact]
        public void GetAllByBankAccountId_Should_Return_All_Operations_Types()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();

            var user = DataBaseHelper.Instance.UseSammyUser(dbName);
            var bankAccount = DataBaseHelper.Instance.UseSammyBankAccount(dbName, user.Id);

            DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationTypeRepository = context.GetOperationTypeRepository();

                var operationTypes = operationTypeRepository.GetAllByBankAccountId(bankAccount.Id);

                Assert.Equal(3, operationTypes.Count());
            }
        }
    }
}
