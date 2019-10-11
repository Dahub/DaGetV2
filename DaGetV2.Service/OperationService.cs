namespace DaGetV2.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
                throw  new DaGetServiceException("Le format de la date de départ doit être de la fome yyyyMMdd (exemple : 20190618)");
            }
            if(!DateTime.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var convertedEndDate))
            {
                throw new DaGetServiceException("Le format de la date de fin doit être de la fome yyyyMMdd (exemple : 20190618)");
            }

            var operationRepository = context.GetOperationRepository();
            return operationRepository.GetAll(bankAccountId, convertedStartDate, convertedEndDate, null, null).ToList().ToDto();
        }
    }
}