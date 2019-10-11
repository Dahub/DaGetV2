namespace DaGetV2.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.Dal.Interface;
    using DTO;
    using Interface;

    public class OperationService : BaseService, IOperationService
    {
        public IEnumerable<OperationDto> GetOperationsWithCriterias(IContext context, string userName, Guid bankAccountId, DateTime startDate, DateTime endDate)
        {
            var bankAccountRepository = context.GetBankAccountRepository();
            var bankAccount = bankAccountRepository.GetById(bankAccountId);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount);

            var operationRepository = context.GetOperationRepository();
            return operationRepository.GetAll(bankAccountId, startDate, endDate, null, null).ToList().ToDto();
        }
    }
}