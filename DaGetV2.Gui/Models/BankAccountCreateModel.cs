using System;
using System.Collections.Generic;

namespace DaGetV2.Gui.Models
{
    public class BankAccountCreateModel
    {
        public Guid? Id { get; set; }

        public string Wording { get; set; }

        public decimal InitialBalance { get; set; }

        public Guid? BankAccountTypeId { get; set; }

        public IDictionary<Guid, string> BankAccountTypes { get; set; }

        public IDictionary<Guid, string> OperationTypes { get; set; }
    }
}
