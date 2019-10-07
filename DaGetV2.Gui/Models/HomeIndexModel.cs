using System.Collections.Generic;

namespace DaGetV2.Gui.Models
{
    public class HomeIndexModel
    {
        public HomeIndexModel()
        {
            PersonnalsCurrentBankAccounts = new List<BankAccountSummary>();
            PersonnalsSavingBankAccounts = new List<BankAccountSummary>();
            SharedsCurrentBankAccounts = new List<BankAccountSummary>();
            SharedsSavingBankAccounts = new List<BankAccountSummary>();
        }

        public IEnumerable<BankAccountSummary> PersonnalsCurrentBankAccounts { get; set; }

        public IEnumerable<BankAccountSummary> PersonnalsSavingBankAccounts { get; set; }

        public IEnumerable<BankAccountSummary> SharedsCurrentBankAccounts { get; set; }

        public IEnumerable<BankAccountSummary> SharedsSavingBankAccounts { get; set; }
    }

    public class BankAccountSummary
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }

        public string Wording { get; set; }

        public string BankAccountType { get; set; }

        public bool IsOwner { get; set; }

        public bool IsReadOnly { get; set; }
    }
}
