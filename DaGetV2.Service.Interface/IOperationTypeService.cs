namespace DaGetV2.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.Dal.Interface;
    using DaGetV2.Service.DTO;

    public interface IOperationTypeService
    {
        IEnumerable<OperationTypeDto> GetDefaultsOperationTypes();

        IEnumerable<OperationTypeDto> GetBankAccountOperationsType(IContext context, string userName, Guid bankAccountId);
    }
}
