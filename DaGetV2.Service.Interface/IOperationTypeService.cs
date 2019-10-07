using System;
using System.Collections.Generic;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.DTO;

namespace DaGetV2.Service.Interface
{
    public interface IOperationTypeService
    {
        IEnumerable<OperationTypeDto> GetDefaultsOperationTypes();

        IEnumerable<OperationTypeDto> GetBankAccountOperationsType(IContext context, string userName, Guid bankAccountId);
    }
}
