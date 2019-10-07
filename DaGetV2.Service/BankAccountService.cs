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
        public Guid Create(IContext context, string userName, CreateBankAccountDto toCreateBankAccount)
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
                OpeningBalance = toCreateBankAccount.InitialBalance.Value,
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

        public BankAccountDto GetById(IContext context, string userName, Guid id)
        {
            var bankAccountRepositoy = context.GetBankAccountRepository();
            var bankAccount = bankAccountRepositoy.GetById(id);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount);

            return bankAccount.ToDto(userName);
        }

        public void Update(IContext context, string userName, UpdateBankAccountDto toEditBankAccount)
        {
            var bankAccountRepository = context.GetBankAccountRepository();
            var operationTypeRepository = context.GetOperationTypeRepository();
            var bankAccountTypeRepository = context.GetBankAccountTypeRepository();
            var bankAccount = bankAccountRepository.GetById(toEditBankAccount.Id.Value);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount, true);

            var bankAccountType = bankAccountTypeRepository.GetById(toEditBankAccount.BankAccountTypeId.Value);

            if (bankAccountType == null)
            {
                throw new DaGetNotFoundException("Type de compte en banque inconnu");
            }
           
            if(toEditBankAccount.InitialBalance.HasValue && bankAccount.OpeningBalance != toEditBankAccount.InitialBalance.Value)
            {
                bankAccount.OpeningBalance = toEditBankAccount.InitialBalance.Value;
                bankAccount.Balance = toEditBankAccount.InitialBalance.Value;
                RebuildBalance(context, bankAccount);                
            }

            bankAccount.ModificationDate = DateTime.Now;
            bankAccount.BankAccountTypeId = toEditBankAccount.BankAccountTypeId.Value;
            bankAccount.Wording = toEditBankAccount.Wording;

            // manage operations types
            var operationTypes = operationTypeRepository.GetAllByBankAccountId(bankAccount.Id);
            var toDeleteOperationsTypes = operationTypes.Where(ot => !(toEditBankAccount.OperationsTypes.Where(eot => eot.Key.HasValue).Select(eot => eot.Key.Value).Contains(ot.Id)));
            var toUpdateOperationsTypes = operationTypes.Where(ot => toEditBankAccount.OperationsTypes.Where(eot => eot.Key.HasValue).Select(eot => eot.Key.Value).Contains(ot.Id));
            var newOperationsTypes = toEditBankAccount.OperationsTypes.Where(eot => !eot.Key.HasValue).Select(eot =>
                new OperationType()
                {
                    BankAccountId = bankAccount.Id,
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Wording = eot.Value
                });

            foreach(var newOperationType in newOperationsTypes)
            {
                operationTypeRepository.Add(newOperationType);
            }
          
            foreach(var toUpdateOperationType in toUpdateOperationsTypes)
            {
                var newWording = toEditBankAccount.OperationsTypes.
                    Where(eot => eot.Key.HasValue && eot.Key.Value.Equals(toUpdateOperationType.Id)).
                    Select(eot => eot.Value).Single();
                if(toUpdateOperationType.Wording != newWording)
                {
                    toUpdateOperationType.Wording = newWording;
                    toUpdateOperationType.ModificationDate = DateTime.Now;
                    operationTypeRepository.Update(toUpdateOperationType);
                }
            }
         
            foreach(var toDeleteOperationType in toDeleteOperationsTypes)
            {
                if(operationTypeRepository.OperationTypeHasOperations(toDeleteOperationType.Id))
                {
                    throw new DaGetServiceException($"Le type d'opération {toDeleteOperationType.Wording} possède des opérations associées");
                }
                operationTypeRepository.Delete(toDeleteOperationType);
            }

            context.Commit();
        }

        private void RebuildBalance(IContext context, BankAccount bankAccount)
        {
            var operationRepository = context.GetOperationRepository();

            foreach (var operation in operationRepository.GetAllByBankAccountId(bankAccount.Id))
            {
                bankAccount.Balance += operation.Amount;
            }
        }
    }
}
