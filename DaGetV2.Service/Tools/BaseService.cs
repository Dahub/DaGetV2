namespace DaGetV2.Service
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using DaGetV2.Dal.Interface;
    using DaGetV2.Domain;

    public abstract class BaseService
    {
        public AppConfiguration Configuration { get; set; }

        protected void CheckIfUserCanAccesBankAccount(
            IContext context, 
            string userName, 
            Guid bankAccountId,
            bool wantRightToWrite = false, 
            bool mustBeOwner = false)
        {
            var bankAccountRepository = context.GetBankAccountRepository();
            var bankAccount = bankAccountRepository.GetById(bankAccountId);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount, wantRightToWrite, mustBeOwner);
        }

        protected void CheckIfUserCanAccesBankAccount(
            IContext context, 
            string userName, 
            BankAccount bankAccount, 
            bool wantRightToWrite = false, 
            bool mustBeOwner = false)
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

            if (mustBeOwner)
            {
                if (!userBankAccount.IsOwner)
                {
                    throw new DaGetUnauthorizedException("Opération interdite");
                }
            }
        }

        protected void Validate<T>(T toValidate, Func<T, IList<ValidationResult>> extendValidate)
        {
            if (toValidate == null)
            {
                throw new DaGetServiceException("Aucune données reçues");
            }

            var results = extendValidate(toValidate);
            if (results == null)
            {
                results = new List<ValidationResult>();
            }

            var context = new ValidationContext(toValidate, null, null);

            if (!Validator.TryValidateObject(toValidate, context, results, true) || results.Count > 0)
            {
                var stringBuilder = new StringBuilder();

                foreach (var error in results)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new DaGetServiceException(stringBuilder.ToString());
            }
        }

        protected void Validate(object toValidate)
        {
            Validate(toValidate, x => new List<ValidationResult>());
        }
    }
}
