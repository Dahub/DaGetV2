using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DaGetV2.Gui.Models
{
    public class BankAccountModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Le nom du compte est obligatoire")]
        [DisplayName("Libellé")]
        public string Wording { get; set; }

        [Required(ErrorMessage = "Le montant initial est obligatoire")]
        [DisplayName("Montant intial")]
        public decimal InitialBalance { get; set; }

        [Required(ErrorMessage = "Le type de compte est obligatoire")]
        [DisplayName("Type de compte")]
        public Guid? BankAccountTypeId { get; set; }

        public IDictionary<Guid, string> BankAccountTypes { get; set; }

        public IEnumerable<KeyValuePair<Guid?, string>> OperationTypes { get; set; }
    }
}
