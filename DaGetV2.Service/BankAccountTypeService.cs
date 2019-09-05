using System.Collections.Generic;
using System.Linq;
using DaGetV2.Dal.Interface;
using DaGetV2.Service.DTO;
using DaGetV2.Service.Interface;

namespace DaGetV2.Service
{
    public class BankAccountTypeService : BaseService, IBankAccountTypeService
    {
        public IEnumerable<BankAccountTypeDto> GetAll(IContext context)
        {
            var bankAccountTypeRepositoy = context.GetBankAccountTypeRepository();

            return bankAccountTypeRepositoy.GetAll().ToList().ToDto();
        }
    }
}
