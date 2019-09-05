using System;
using System.Collections.Generic;
using System.Linq;
using DaGetV2.Service.DTO;
using DaGetV2.Service.Interface;

namespace DaGetV2.Service
{
    public class OperationTypeService : BaseService, IOperationTypeService
    {
        public IEnumerable<OperationTypeDto> GetDefaultsOperationTypes()
        {
            return Configuration.DefaultsOperationTypes.Select(ot => new OperationTypeDto()
            {
                Id = Guid.NewGuid(),
                Wording = ot
            });
        }
    }
}
