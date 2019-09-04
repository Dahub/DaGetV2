using System.Collections.Generic;

namespace DaGetV2.Gui.Models
{
    public class HomeIndexModel
    {
        public HomeIndexModel()
        {
            BankAccountSummaries = new List<BankAccountSummary>();
        }

        public IEnumerable<BankAccountSummary> BankAccountSummaries { get; set; }
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
