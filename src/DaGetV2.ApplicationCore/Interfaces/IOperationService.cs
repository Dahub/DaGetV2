namespace DaGetV2.ApplicationCore.Interfaces
{
    using System;
    using System.Collections.Generic;
    using DTO;

    public interface IOperationService
    {
        IEnumerable<OperationDto> GetOperationsWithCriterias(
            IContext context,
            string userName,
            Guid bankAccountId,
            string startDate,
            string endDate);

        void Update(IContext context, string userName, UpdateOperationDto updateOperationDto);
    }
}