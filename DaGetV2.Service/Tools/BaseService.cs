using DaGetV2.Dal.Interface;
using DaGetV2.Domain;

namespace DaGetV2.Service
{
    public abstract class BaseService
    {
        public AppConfiguration Configuration { get; set; }

        protected void CheckIfUserCanAccesBankAccount(IContext context, string userName, BankAccount bankAccount, bool wantRightToWrite = false)
        {
            var userBankAccountRepository = context.GetUserBankAccountRepository();
            var userRepository = context.GetUserRepository();

            var user = userRepository.GetByUserName(userName);

            if (bankAccount == null)
            {
                throw new DaGetNotFoundException("Compte en banque inconnu");
            }

            if (user == null)
            {
                throw new DaGetUnauthorizedException("Utilisateur inconnu");
            }

            var userBankAccount = userBankAccountRepository.GetByIdUserAndIdBankAccount(user.Id, bankAccount.Id);

            if (userBankAccount == null)
            {
                throw new DaGetNotFoundException("Compte en banque inconnu");
            }

            if (wantRightToWrite)
            {
                if (!userBankAccount.IsOwner || userBankAccount.IsReadOnly)
                {
                    throw new DaGetUnauthorizedException("Opération interdite");
                }
            }
        }
    }
}
