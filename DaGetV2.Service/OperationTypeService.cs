namespace DaGetV2.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Dal.Interface;
    using DTO;
    using Interface;

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
