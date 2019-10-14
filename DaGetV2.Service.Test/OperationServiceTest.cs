namespace DaGetV2.Service.Test
{
    using System;
    using System.Linq;
    using DTO;
    using Shared.TestTool;
    using Xunit;

    public class OperationServiceTest
    {
        [Theory]
        [InlineData("20190101", "01012019")]
        [InlineData("01012019", "20190101")]
        public void GetOperationsWithCriterias_With_Bad_Date_Format_Should_Throw_DaGet_Service_Exception(string startDate, string endDate)
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetServiceException>(() => operationService.GetOperationsWithCriterias(
                    context,
                    user.UserName,
                    bankAccount.Id,
                    startDate,
                    endDate));
            }
        }

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
                    DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"),
                    DateTime.Now.ToString("yyyyMMdd")));
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
                    operationService.GetOperationsWithCriterias(context, user.UserName, bankAccount.Id, startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));

                Assert.NotNull(operationsFromService);
                Assert.NotEmpty(operationsFromService);
                Assert.Equal(2, actual: operationsFromService.Count());
                Assert.NotNull(operationsFromService.SingleOrDefault(o => o.Id.Equals(innerOperationOne.Id)));
                Assert.NotNull(operationsFromService.SingleOrDefault(o => o.Id.Equals(innerOperationTwo.Id)));
                Assert.Null(operationsFromService.SingleOrDefault(o => o.Id.Equals(beforeOperation.Id)));
                Assert.Null(operationsFromService.SingleOrDefault(o => o.Id.Equals(afterOperation.Id)));
            }
        }

        [Fact]
        public void Update_When_Operation_Bank_Account_Is_Read_Only_Should_Throw_DaGetUnauthorizedException()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var userReadOnly = DataBaseHelper.Instance.UseNewUser(dbName);
            DataBaseHelper.Instance.UseNewUserBankAccount(dbName, userReadOnly.Id, bankAccount.Id, false, true);
            var operationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetUnauthorizedException>(() => operationService.Update(context, userReadOnly.UserName, new UpdateOperationDto()
                {
                    Id = operation.Id,
                    Wording = operation.Wording,
                    Amount = operation.Amount,
                    IsClosed = true,
                    OperationDate = DateTime.Now,
                    OperationTypeId = operationType.Id
                }));
            }
        }

        [Fact]
        public void Update_When_Operation_Bank_Account_Is_Own_By_Another_User_Should_Throw_DaGetNotFoundException()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var anotherUser = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, anotherUser.Id, bankAccountType.Id);
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var operationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetNotFoundException>(() => operationService.Update(context, user.UserName, new UpdateOperationDto()
                {
                    Id = operation.Id,
                    Wording = operation.Wording,
                    Amount = operation.Amount,
                    IsClosed = true,
                    OperationDate = DateTime.Now,
                    OperationTypeId = operationType.Id
                }));
            }
        }

        [Fact]
        public void Update_When_Operation_Dont_Exists_Should_Throw_DaGetNotFoundException()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var operationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetNotFoundException>(() => operationService.Update(context, user.UserName, new UpdateOperationDto()
                {
                    Id = Guid.NewGuid(),
                    Wording = operation.Wording,
                    Amount = operation.Amount,
                    IsClosed = true,
                    OperationDate = DateTime.Now,
                    OperationTypeId = operationType.Id
                }));
            }
        }

        [Fact]
        public void Update_When_Operation_Exists_In_Another_Bank_Account_Should_Throw_DaGetNotFoundException()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var anotherBankAccount =
                DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var operationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);
            var operationTypeFromAnotherBankAcount =
                DataBaseHelper.Instance.UseNewOperationType(dbName, anotherBankAccount.Id);

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetNotFoundException>(() => operationService.Update(context, user.UserName, new UpdateOperationDto()
                {
                    Id = operation.Id,
                    Wording = operation.Wording,
                    Amount = operation.Amount,
                    IsClosed = operation.IsClosed,
                    OperationDate = operation.OperationDate,
                    OperationTypeId = operationTypeFromAnotherBankAcount.Id
                }));
            }
        }

        [Fact]
        public void Update_When_Operation_Type_Dont_Exists_Should_Throw_DaGetNotFoundException()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            var operation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);

            var operationService = new OperationService();

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                Assert.Throws<DaGetNotFoundException>(() => operationService.Update(context, user.UserName, new UpdateOperationDto()
                {
                    Id = operation.Id,
                    Wording = operation.Wording,
                    Amount = operation.Amount,
                    IsClosed = true,
                    OperationDate = DateTime.Now,
                    OperationTypeId = Guid.NewGuid()
                }));
            }
        }

        [Fact]
        public void Update_With_Correct_Informations_Should_Update_Operation()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var newOperationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);

            var operationService = new OperationService();

            var newWording = Guid.NewGuid().ToString();
            var newAmount = operation.Amount * 2;
            var newIsClosed = !operation.IsClosed;
            var newOperationDate = operation.OperationDate.AddMonths(-1);
            var newOperationTypeId = newOperationType.Id;

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                operationService.Update(context, user.UserName, new UpdateOperationDto()
                {
                    Id = operation.Id,
                    Wording = newWording,
                    Amount = newAmount,
                    IsClosed = newIsClosed,
                    OperationDate = newOperationDate,
                    OperationTypeId = newOperationTypeId
                });
            }

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var operationFromDb = context.Operations.SingleOrDefault(o => o.Id.Equals(operation.Id));

                Assert.NotNull(operationFromDb);
                Assert.Equal(newWording, operationFromDb.Wording);
                Assert.Equal(newAmount, operationFromDb.Amount);
                Assert.Equal(newIsClosed, operationFromDb.IsClosed);
                Assert.Equal(newOperationDate, operationFromDb.OperationDate);
                Assert.Equal(newOperationTypeId, operationFromDb.OperationTypeId);
            }
        }

        [Fact]
        public void Update_When_Amount_Is_Modified_Should_Update_Bank_Account_Balance()
        {
            var dbName = DataBaseHelper.Instance.NewDataBase();
            var user = DataBaseHelper.Instance.UseNewUser(dbName);
            var bankAccountType = DataBaseHelper.Instance.UseNewBankAccountType(dbName);
            var bankAccount = DataBaseHelper.Instance.UseNewBankAccount(dbName, user.Id, bankAccountType.Id);
            DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var newOperationType = DataBaseHelper.Instance.UseNewOperationType(dbName, bankAccount.Id);
            var operation = DataBaseHelper.Instance.UseNewOperation(dbName, bankAccount.Id);

            var operationService = new OperationService();

            var newWording = Guid.NewGuid().ToString();
            var newAmount = operation.Amount * 2;
            var newIsClosed = !operation.IsClosed;
            var newOperationDate = operation.OperationDate.AddMonths(-1);
            var newOperationTypeId = newOperationType.Id;

            var newBankAccountAmount = bankAccount.Balance + newAmount;

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                operationService.Update(context, user.UserName, new UpdateOperationDto()
                {
                    Id = operation.Id,
                    Wording = newWording,
                    Amount = newAmount,
                    IsClosed = newIsClosed,
                    OperationDate = newOperationDate,
                    OperationTypeId = newOperationTypeId
                });
            }

            using (var context = DataBaseHelper.Instance.CreateContext(dbName))
            {
                var bankAccountFromDb = context.BankAccounts.SingleOrDefault(ba => ba.Id.Equals(bankAccount.Id));

                Assert.NotNull(bankAccountFromDb);
                Assert.Equal(newBankAccountAmount, bankAccountFromDb.Balance);

            }
        }
    }
}
