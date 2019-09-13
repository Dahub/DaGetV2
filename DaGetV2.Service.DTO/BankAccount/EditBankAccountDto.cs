using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DaGetV2.Shared.ApiTool;

namespace DaGetV2.Service.DTO
{
    public class EditBankAccountDto : IDto
    {
        [Required(ErrorMessage = "L'id de comtpe est obligatoire")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Type de compte en banque obligatoire")]
        public Guid? BankAccountTypeId { get; set; }

        [Required(ErrorMessage = "Nom de compte en banque obligatoire")]
        public string Wording { get; set; }

        [Required(ErrorMessage = "Montant initial obligatoire")]
        public decimal? InitialBalance { get; set; }

        public IList<KeyValuePair<Guid, string>> OperationsTypes { get; set; }
    }
}
