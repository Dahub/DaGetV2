using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaGetV2.Gui.Models
{
    public class BankAccountCreateModel
    {
        [Required(ErrorMessage = "Le nom du compte est obligatoire")]
        public string Wording { get; set; }

        [Required(ErrorMessage = "Le montant initial est obligatoire")]
        public decimal InitialBalance { get; set; }

        [Required(ErrorMessage = "Le type de compte est obligatoire")]
        public Guid? BankAccountTypeId { get; set; }

        public IDictionary<Guid, string> BankAccountTypes { get; set; }

        public IEnumerable<KeyValuePair<Guid?, string>> OperationTypes { get; set; }
    }
}
