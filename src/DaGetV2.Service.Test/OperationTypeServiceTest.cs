namespace DaGetV2.Service.Test
{
    using System;
    using System.Linq;
    using ApplicationCore.Services;
    using ApplicationCore.Tools;
    using DaGetV2.Shared.TestTool;
    using Xunit;

    public class OperationTypeServiceTest
    {
        [Fact]
        public void GetDefaultsOperationTypes_Should_Return_Default_Operations_Types()
        {
            var defaultsOperationsTypes = new string[] {"type1", "type2", "type3"};

            var operationTypeService = new OperationTypeService()
            {
                Configuration = new AppConfiguration()
                {
                    DefaultsOperationTypes = defaultsOperationsTypes
                }
            };

            var operationsTypes = operationTypeService.GetDefaultsOperationTypes();

            Assert.Equal(defaultsOperationsTypes.Length, operationsTypes.Count());
            foreach (var operationType in defaultsOperationsTypes)
            {
                Assert.NotNull(operationsTypes.SingleOrDefault(ot => ot.Wording.Equals(operationType)));
            }
        }

        [Fact]
        public void GetBankAccountOperationsType_Should_Return_Bank_Account_Operations_Types()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var operationType1 = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operationType2 = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operationType3 = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);

            var operationTypeService = new OperationTypeService();
            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationsTypes = operationTypeService.GetBankAccountOperationsType(context, user.UserName, bankAccount.Id);

                Assert.NotNull(operationsTypes);
                Assert.Equal(3, operationsTypes.Count());
                Assert.NotNull(operationsTypes.SingleOrDefault(ot => ot.Id.Equals(operationType1.Id)));
                Assert.NotNull(operationsTypes.SingleOrDefault(ot => ot.Id.Equals(operationType2.Id)));
                Assert.NotNull(operationsTypes.SingleOrDefault(ot => ot.Id.Equals(operationType3.Id)));
            }
        }
    }
}
