namespace DaGetV2.Domain
{
    using DaGetV2.Domain.Interface;
    using System;
    using System.Collections.Generic;

    public class User : IDomainObject
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public string UserName { get; set; }        

        public ICollection<UserBankAccount> UsersBanksAccounts { get; set; }        
    }
}
