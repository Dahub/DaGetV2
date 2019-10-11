namespace DaGetV2.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.Dal.Interface;
    using DTO;

    public interface IOperationService
    {
        IEnumerable<OperationDto> GetOperationsWithCriterias(
            IContext context,
            string userName,
            Guid bankAccountId,
            string startDate,
            string endDate);
    }
}