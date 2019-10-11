namespace DaGetV2.Service.Test
{
    using System;
    using System.Linq;
    using Shared.TestTool;
    using Xunit;

    public class OperationServiceTest
    {
        [Fact]
        public void GetOperationsWithCriterias_With_Bank_Account_From_Another_User_Should_Throw_DaGet_Unauthorized_Exception()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);

            var badUserName = Guid.NewGuid().ToString();

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetUnauthorizedException>(() => operationService.GetOperationsWithCriterias(
                    context,
                    badUserName,
                    bankAccount.Id,
                    DateTime.Now.AddMonths(-1),
                    DateTime.Now));
            }
        }

        [Fact]
        public void GetOperationsWithCriterias_With_Two_Date_Should_Return_All_Operations_Between_Dates()
        {
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;

            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var operationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var beforeOperation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now.AddMonths(-2));
            var afterOperation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now.AddMonths(+1));
            var innerOperationOne = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now.AddDays(-15));
            var innerOperationTwo = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now.AddDays(-20));

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationsFromService =
                    operationService.GetOperationsWithCriterias(context, user.UserName, bankAccount.Id, startDate, endDate);

                Assert.NotNull(operationsFromService);
                Assert.NotEmpty(operationsFromService);
                Assert.Equal(2, actual: operationsFromService.Count());
                Assert.NotNull(operationsFromService.SingleOrDefault(o => o.Id.Equals(innerOperationOne.Id)));
                Assert.NotNull(operationsFromService.SingleOrDefault(o => o.Id.Equals(innerOperationTwo.Id)));
                Assert.Null(operationsFromService.SingleOrDefault(o => o.Id.Equals(beforeOperation.Id)));
                Assert.Null(operationsFromService.SingleOrDefault(o => o.Id.Equals(afterOperation.Id)));
            }
        }
    }
}
