namespace DaGetV2.ApplicationCore.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using DTO;

    public static class ExtensionMethod
    {
        public static IEnumerable<OperationDto> ToDto(this IEnumerable<Operation> operations)
        {
            if (operations == null)
            {
                yield break;
            }

            foreach (var operation in operations)
            {
                yield return operation.ToDto();
            }
        }

        public static OperationDto ToDto(this Operation operation)
        {
            return new OperationDto()
            {
                Id = operation.Id,
                BankAccountId = operation.BankAccountId,
                Amount = operation.Amount,
                IsClosed = operation.IsClosed,
                IsTransfert = operation.IsTransfert,
                OperationDate = operation.OperationDate,
                OperationTypeId = operation.OperationTypeId,
                OperationTypeWording = operation.OperationType.Wording,
                Wording = operation.Wording
            };
        }

        public static IEnumerable<OperationTypeDto> ToDto(this IEnumerable<OperationType> operationsTypes)
        {
            if (operationsTypes == null)
            {
                yield break;
            }

            foreach (var operationType in operationsTypes)
            {
                yield return operationType.ToDto();
            }
        }

        public static OperationTypeDto ToDto(this OperationType operationType)
        {
            if (operationType == null)
            {
                return null;
            }

            return new OperationTypeDto()
            {
                Id = operationType.Id,
                Wording = operationType.Wording
            };
        }

        public static IEnumerable<BankAccountTypeDto> ToDto(this IEnumerable<BankAccountType> bankAccountTypes)
        {
            if (bankAccountTypes == null)
            {
                yield break;
            }

            foreach (var bat in bankAccountTypes)
            {
                yield return bat.ToDto();
            }
        }

        public static BankAccountTypeDto ToDto(this BankAccountType bankAccountType)
        {
            if (bankAccountType == null)
            {
                return null;
            }

            return new BankAccountTypeDto()
            {
                Id = bankAccountType.Id,
                Wording = bankAccountType.Wording
            };
        }

        public static IEnumerable<BankAccountDto> ToDto(this IEnumerable<BankAccount> bankAccounts, string userName)
        {
            if (bankAccounts == null || String.IsNullOrWhiteSpace(userName))
            {
                yield break;
            }

            foreach (var ba in bankAccounts)
            {
                yield return ba.ToDto(userName);
            }
        }

        public static BankAccountDto ToDto(this BankAccount bankAccount, string userName)
        {
            if (bankAccount == null || String.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            var userBankAccount = bankAccount.UsersBanksAccounts.SingleOrDefault(uba => uba.User.UserName.Equals(userName));

            return new BankAccountDto()
            {
                Id = bankAccount.Id,
                Balance = bankAccount.Balance,
                ActualBalance = bankAccount.ActualBalance,
                InitialBalance = bankAccount.OpeningBalance,
                BankAccountTypeId = bankAccount.BankAccountTypeId.ToString(),
                BankAccountType = bankAccount.BankAccountType.Wording,
                IsOwner = userBankAccount.IsOwner,
                IsReadOnly = userBankAccount.IsReadOnly,
                Wording = bankAccount.Wording
            };
        }
    }
}
