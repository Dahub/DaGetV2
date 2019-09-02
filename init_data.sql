delete from [daget].[BankAccount]
go

declare @userName nvarchar(256)
set @userName = 'david'

declare @userId uniqueIdentifier
select @userId = Id from [daget].[User] where username = @userName

declare @bankAccountTypeId uniqueIdentifier
select @bankAccountTypeId = Id from [daget].[BankAccountType] where Wording = 'courant'

declare @idBankAccount uniqueIdentifier
set @idBankAccount = newid()

insert into [daget].[BankAccount] (id, CreationDate, ModificationDate, FK_BankAccountType, Balance, OpeningBalance, Wording)
values (@idBankAccount, getdate(), getdate(), @bankAccountTypeId, 0, 0, 'test bank account')

insert into [daget].[UserBankAccount] (Id, CreationDate, ModificationDate, FK_User, FK_BankAccount, IsOwner, IsReadOnly)
values (newid(), getdate(), getdate(), @userId, @idBankAccount, 1, 0)
go

