namespace DaGetV2.ApplicationCore.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DaGetV2.Shared.ApiTool;

    public class CreateBankAccountDto : IDto
    {
        [Required(ErrorMessage = "Type de compte en banque obligatoire")]
        public Guid? BankAccountTypeId { get; set; }

        [Required(ErrorMessage = "Nom de compte en banque obligatoire")]
        public string Wording { get; set; }

        [Required(ErrorMessage = "Montant initial obligatoire")]
        public decimal? InitialBalance { get; set; }

        public IEnumerable<string> OperationsTypes { get; set; }
    }
}
