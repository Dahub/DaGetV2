using DaGetV2.Domain.Interface;
using System;
using System.Collections.Generic;

namespace DaGetV2.Domain
{
    public class User : IDomainObject
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime LastConnexionDate { get; set; }
        public ICollection<UserBankAccount> UsersBanksAccounts { get; set; }
    }
}
