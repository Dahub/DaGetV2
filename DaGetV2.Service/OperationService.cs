namespace DaGetV2.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using DaGetV2.Dal.EF;
    using DaGetV2.Dal.Interface;
    using DTO;
    using Interface;

    public class OperationService : BaseService, IOperationService
    {
        public IEnumerable<OperationDto> GetOperationsWithCriterias(IContext context, string userName, Guid bankAccountId, string startDate, string endDate)
        {
            var bankAccountRepository = context.GetBankAccountRepository();
            var bankAccount = bankAccountRepository.GetById(bankAccountId);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount);

            if (!DateTime.TryParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var convertedStartDate))
            {
                throw  new DaGetServiceException("Le format de la date de départ doit être de la forme yyyyMMdd (exemple : 20190618)");
            }
            if(!DateTime.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var convertedEndDate))
            {
                throw new DaGetServiceException("Le format de la date de fin doit être de la forme yyyyMMdd (exemple : 20190618)");
            }

            var operationRepository = context.GetOperationRepository();
            return operationRepository.GetAll(bankAccountId, convertedStartDate, convertedEndDate, null, null).ToList().ToDto();
        }

        public void Update(DaGetContext context, string userName, UpdateOperationDto updateOperationDto)
        {
            var operationRepository = context.GetOperationRepository();
            var operationTypeRepository = context.GetOperationTypeRepository();
            var bankAccountRepository = context.GetBankAccountRepository();

            var operation = operationRepository.GetById(updateOperationDto.Id);
            if (operation == null)
            {
                throw new DaGetNotFoundException("Opération inconnue");
            }

            var bankAccount = bankAccountRepository.GetById(operation.BankAccountId);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount, true, true);

            var operationType = operationTypeRepository.GetById(updateOperationDto.OperationTypeId);
            if (operationType == null || !operationType.BankAccountId.Equals((bankAccount.Id)))
            {
                throw new DaGetNotFoundException("Type d'opération inconnue");
            }

            bankAccount.Balance -= operation.Amount;
            bankAccount.Balance += updateOperationDto.Amount;

            operation.Amount = updateOperationDto.Amount;
            operation.IsClosed = updateOperationDto.IsClosed;
            operation.OperationDate = updateOperationDto.OperationDate;
            operation.OperationTypeId = updateOperationDto.OperationTypeId;
            operation.Wording = updateOperationDto.Wording;

            operationRepository.Update(operation);
            bankAccountRepository.Update(bankAccount);

            context.Commit();
        }
    }
}