namespace DaGetV2.ApplicationCore.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Exceptions;
    using Interfaces;
    using Domain;
    using DTO;
    using Specifications;
    using Tools;

    public class BankAccountService : BaseService, IBankAccountService
    {
        public Guid Create(IContext context, string userName, CreateBankAccountDto toCreateBankAccount)
        {
            Validate(toCreateBankAccount);

            var bankAccountId = Guid.NewGuid();

            var bankAccountRepository = context.GetRepository<BankAccount>();
            var operationTypeRepository = context.GetRepository<OperationType>();
            var userBankAccountRepository = context.GetRepository<UserBankAccount>();
            var userRepository = context.GetRepository<User>();

            var user = userRepository.SingleOrDefault(new UserByUserNameSpecification(userName));

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

        public void DeleteBankAccountById(IContext context, string userName, Guid bankAccountId)
        {
            var operationRepository = context.GetRepository<Operation>();
            var bankAccountRepository = context.GetRepository<BankAccount>();
            var operationTypeRepository = context.GetRepository<OperationType>();
            var userBankAccountRepository = context.GetRepository<UserBankAccount>();

            var bankAccount = bankAccountRepository.GetById(bankAccountId);

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount, true, true);

            foreach (var userBankAccount in userBankAccountRepository.List(new UserBankAccountByBankAccountIdSpecification(bankAccountId)))
            {
                userBankAccountRepository.Delete(userBankAccount);
            }

            foreach (var operationType in operationTypeRepository.List(new OperationTypeByBankAccountIdSpecification(bankAccountId)))
            {
                operationTypeRepository.Delete(operationType);
            }

            foreach (var operation in operationRepository.List(new OperationByBankAccountId(bankAccountId)))
            {
                operationRepository.Delete(operation);
            }

            bankAccountRepository.Delete(bankAccount);

            context.Commit();
        }

        public IEnumerable<BankAccountDto> GetAll(IContext context, string userName)
        {
            var userBankAccountRepositoy = context.GetRepository<UserBankAccount>();
            return userBankAccountRepositoy.List(new UserBankAccountByUserNameSpecification(userName)).Select(uba => uba.BankAccount).ToDto(userName);
        }

        public BankAccountDto GetById(IContext context, string userName, Guid bankAccountId)
        {
            var bankAccountRepositoy = context.GetRepository<BankAccount>();
            var bankAccount = bankAccountRepositoy.SingleOrDefault(new BankAccountByIdWithBankAccountTypeSpecification(bankAccountId));

            CheckIfUserCanAccesBankAccount(context, userName, bankAccount);

            return bankAccount.ToDto(userName);
        }

        public void Update(IContext context, string userName, UpdateBankAccountDto toEditBankAccount)
        {
            Validate(toEditBankAccount);

            var bankAccountRepository = context.GetRepository<BankAccount>();
            var operationTypeRepository = context.GetRepository<OperationType>();
            var bankAccountTypeRepository = context.GetRepository<BankAccountType>();
            var operationRepository = context.GetRepository<Operation>();

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
            var operationTypes = operationTypeRepository.List(new OperationTypeByBankAccountIdSpecification(bankAccount.Id));
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
                if(operationRepository.List(new FirstOperationByOperationTypeIdSpecification(toDeleteOperationType.Id)).Any())
                {
                    throw new DaGetServiceException($"Le type d'opération {toDeleteOperationType.Wording} possède des opérations associées");
                }
                operationTypeRepository.Delete(toDeleteOperationType);
            }

            context.Commit();
        }

        private void RebuildBalance(IContext context, BankAccount bankAccount)
        {
            var operationRepository = context.GetRepository<Operation>();

            foreach (var operation in operationRepository.List(new OperationByBankAccountId(bankAccount.Id)))
            {
                bankAccount.Balance += operation.Amount;
            }
        }
    }
}
