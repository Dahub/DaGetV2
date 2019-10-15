namespace DaGetV2.ApplicationCore.Domain
{
    using System;
    using System.Collections.Generic;
    using DaGetV2.ApplicationCore.Interfaces;

    public class User : IDomainObject
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModificationDate { get; set; }

        public string UserName { get; set; }        

        public ICollection<UserBankAccount> UsersBanksAccounts { get; set; }        
    }
}
