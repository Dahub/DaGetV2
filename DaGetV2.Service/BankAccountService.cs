using System;
using System.Collections.Generic;
using System.Linq;
using DaGetV2.Dal.Interface;
using DaGetV2.Domain;
using DaGetV2.Service.DTO;
using DaGetV2.Service.Interface;

namespace DaGetV2.Service
{
    public class BankAccountService : BaseService, IBankAccountService
    {
        public int Add(IContext context, BankAccount toCreate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BankAccountDto> GetAll(IContext context, string userName)
        {
            var bankAccountRepositoy = context.GetBankAccountRepository();

            return bankAccountRepositoy.GetAllByUser(userName).ToList().ToDto(userName);
        }
    }
}
