namespace DaGetV2.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.Dal.Interface;
    using DaGetV2.Service.DTO;
    using DaGetV2.Service.Interface;

    public class OperationTypeService : BaseService, IOperationTypeService
    {
        public IEnumerable<OperationTypeDto> GetDefaultsOperationTypes()
            => Configuration.DefaultsOperationTypes.Select(ot => new OperationTypeDto()
            {
                Id = Guid.NewGuid(),
                Wording = ot
            });

        public IEnumerable<OperationTypeDto> GetBankAccountOperationsType(IContext context, string userName, Guid bankAccountId)
            => context.GetOperationTypeRepository().GetAllByBankAccountId(bankAccountId).ToList().ToDto();
    }
}
