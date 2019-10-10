namespace DaGetV2.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using DaGetV2.Dal.Interface;
    using DaGetV2.Service.DTO;
    using DaGetV2.Service.Interface;

    public class BankAccountTypeService : BaseService, IBankAccountTypeService
    {
        public IEnumerable<BankAccountTypeDto> GetAll(IContext context)
            => context.GetBankAccountTypeRepository().GetAll().ToList().ToDto();
    }
}
