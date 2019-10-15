namespace DaGetV2.ApplicationCore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Domain;
    using DTO;
    using Specifications;
    using Tools;

    public class OperationTypeService : BaseService, IOperationTypeService
    {
        public IEnumerable<OperationTypeDto> GetDefaultsOperationTypes()
            => Configuration.DefaultsOperationTypes.Select(ot => new OperationTypeDto()
            {
                Id = Guid.NewGuid(),
                Wording = ot
            });

        public IEnumerable<OperationTypeDto> GetBankAccountOperationsType(IContext context, string userName, Guid bankAccountId)
            => context.GetRepository<OperationType>().List(new OperationTypeByBankAccountIdSpecification(bankAccountId)).ToList().ToDto();
    }
}
