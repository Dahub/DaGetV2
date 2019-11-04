namespace DaGetV2.ApplicationCore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Exceptions;
    using Interfaces;
    using Domain;
    using DTO;
    using Specifications;
    using Tools;

    public class OperationService : BaseService, IOperationService
    {
        public IEnumerable<OperationDto> GetOperationsWithCriterias(IContext context, string userName, Guid bankAccountId, string startDate, string endDate)
        {
            var bankAccountRepository = context.GetRepository<BankAccount>();
            var bankAccount = bankAccountRepository.GetById(bankAccountId);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount);

            if (!DateTime.TryParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var convertedStartDate))
            {
                throw new DaGetServiceException("Le format de la date de départ doit être de la forme yyyyMMdd (exemple : 20190618)");
            }
            if (!DateTime.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var convertedEndDate))
            {
                throw new DaGetServiceException("Le format de la date de fin doit être de la forme yyyyMMdd (exemple : 20190618)");
            }

            var operationRepository = context.GetRepository<Operation>();
            return operationRepository.List(new OperationsFilteredSpecification(bankAccountId, convertedStartDate, convertedEndDate, null, null)).ToList().ToDto();
        }

        public void Update(IContext context, string userName, UpdateOperationDto updateOperationDto)
        {
            var operationRepository = context.GetRepository<Operation>();
            var operationTypeRepository = context.GetRepository<OperationType>();
            var bankAccountRepository = context.GetRepository<BankAccount>();

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

            bankAccount.ActualBalance -= operation.Amount;
            bankAccount.ActualBalance += updateOperationDto.Amount;

            if (operation.IsClosed != updateOperationDto.IsClosed)
            {
                if (updateOperationDto.IsClosed)
                {
                    bankAccount.ActualBalance += updateOperationDto.Amount;
                }
                else
                {
                    bankAccount.ActualBalance -= updateOperationDto.Amount;
                }
            }

            operation.Amount = updateOperationDto.Amount;
            operation.IsClosed = updateOperationDto.IsClosed;
            operation.OperationDate = updateOperationDto.OperationDate;
            operation.OperationTypeId = updateOperationDto.OperationTypeId;
            operation.Wording = updateOperationDto.Wording;

            operationRepository.Update(operation);
            bankAccountRepository.Update(bankAccount);

            context.Commit();
        }

        public OperationDto GetById(IContext context, string userName, Guid operationId)
        {
            var operationRepository = context.GetRepository<Operation>();

            var operation = operationRepository.SingleOrDefault(new OperationByIdWithOperationTypeSpecification(operationId));

            if (operation == null)
            {
                throw new DaGetNotFoundException("Impossible de trouver l'opération");
            }

            CheckIfUserCanAccesBankAccount(context, userName, operation.BankAccountId, false, false);

            return operation.ToDto();
        }
    }
}