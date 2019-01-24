using DaGetV2.Dal.Interface;
using DaGetV2.Domain;

namespace DaGetV2.Service.Interface
{
    public interface IBankAccountService
    {
        int CreateBankAccount(IContext context, BankAccount toCreate);
    }
}
