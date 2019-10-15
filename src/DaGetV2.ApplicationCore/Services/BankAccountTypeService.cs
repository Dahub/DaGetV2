namespace DaGetV2.ApplicationCore.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Domain;
    using DTO;
    using Tools;

    public class BankAccountTypeService : BaseService, IBankAccountTypeService
    {
        public IEnumerable<BankAccountTypeDto> GetAll(IContext context)
            => context.GetRepository<BankAccountType>().ListAll().ToList().ToDto();
    }
}
