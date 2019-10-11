namespace DaGetV2.Dal.EF.Test
{
    using System;
    using System.Linq;
    using Shared.TestTool;
    using Xunit;

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

        [Fact]
        public void GetAll_Should_Return_All_Operations_With_Criterias()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var operationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);

            DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now);
            DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now);
            DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationRepository = context.GetOperationRepository();

                var operations = operationRepository.GetAll(bankAccount.Id, DateTime.Now.AddMonths(-1),
                    DateTime.Now.AddMonths(1), null, null);

                Assert.NotNull(operations);
                Assert.NotEmpty(operations);
                Assert.Equal(3, operations.Count());
            }
        }

        [Fact]
        public void GetAll_Should_Return_All_Operations_With_Bank_Account_And_Operation_Type_Loaded()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var operationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);

            DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id, operationType.Id, DateTime.Now);

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationRepository = context.GetOperationRepository();

                var operations = operationRepository.GetAll(bankAccount.Id, DateTime.Now.AddMonths(-1),
                    DateTime.Now.AddMonths(1), null, null);

                var operation = operations.FirstOrDefault();

                Assert.NotNull(operation);
                Assert.NotNull(operation.BankAccount);
                Assert.NotNull(operation.OperationType);
            }
        }
    }
}
