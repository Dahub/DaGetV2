namespace DaGetV2.ApplicationCore.Interfaces
{
    using System;
    using System.Collections.Generic;
    using DTO;

    public interface IOperationTypeService
    {
        IEnumerable<OperationTypeDto> GetDefaultsOperationTypes();

        IEnumerable<OperationTypeDto> GetBankAccountOperationsType(IContext context, string userName, Guid bankAccountId);
    }
}
