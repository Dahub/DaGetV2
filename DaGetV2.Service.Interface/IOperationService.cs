namespace DaGetV2.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.Dal.Interface;
    using Dal.EF;
    using DTO;

    public interface IOperationService
    {
        IEnumerable<OperationDto> GetOperationsWithCriterias(
            IContext context,
            string userName,
            Guid bankAccountId,
            string startDate,
            string endDate);

        void Update(DaGetContext context, string userName, UpdateOperationDto updateOperationDto);
    }
}