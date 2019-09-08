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
        public Guid Add(IContext context, string userName, CreateBankAccountDto toCreateBankAccount)
        {
            var bankAccountId = Guid.NewGuid();

            var bankAccountRepository = context.GetBankAccountRepository();
            var operationTypeRepository = context.GetOperationTypeRepository();
            var userBankAccountRepository = context.GetUserBankAccountRepository();
            var userRepository = context.GetUserRepository();

            var user = userRepository.GetByUserName(userName);

            if(user == null)
            {
                throw new DaGetUnauthorizedException("Utilisateur inconnu");
            }

            bankAccountRepository.Add(new BankAccount()
            {
                Balance = toCreateBankAccount.InitialBalance.Value,
                BankAccountTypeId = toCreateBankAccount.BankAccountTypeId.Value,
                Id = bankAccountId,
                Wording = toCreateBankAccount.Wording
            });

            foreach (var operationType in toCreateBankAccount.OperationsTypes)
            {
                operationTypeRepository.Add(new OperationType()
                {
                    Id = Guid.NewGuid(),
                    BankAccountId = bankAccountId,
                    Wording = operationType
                }); 
            }

            userBankAccountRepository.Add(new UserBankAccount()
            {
                BankAccountId = bankAccountId,
                Id = Guid.NewGuid(),
                IsOwner = true,
                IsReadOnly = false,
                UserId = user.Id
            });

            context.Commit();

            return bankAccountId;
        }

        public IEnumerable<BankAccountDto> GetAll(IContext context, string userName)
        {
            var bankAccountRepositoy = context.GetBankAccountRepository();

            return bankAccountRepositoy.GetAllByUser(userName).ToList().ToDto(userName);
        }
    }
}
