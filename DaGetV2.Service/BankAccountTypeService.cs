namespace DaGetV2.Service
{
    using System.Collections.Generic;
    using System.Linq;
    using Dal.Interface;
    using DTO;
    using Interface;

    public class BankAccountTypeService : BaseService, IBankAccountTypeService
    {
        public IEnumerable<BankAccountTypeDto> GetAll(IContext context)
            => context.GetBankAccountTypeRepository().GetAll().ToList().ToDto();
    }
}
