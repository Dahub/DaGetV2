namespace DaGetV2.Gui.Models
{
    using System;
    using System.Collections.Generic;

    public class BankAccountDetailModel : ModelBase
    {
        public BankAccountDetailModel()
        {
            Operations = new List<BankAccountDetailOperationModel>();
        }

        public string BankAccountId { get; set; }

        public string BankAccountWording { get; set; }

        public decimal BankAccountBalance { get; set; }

        public decimal Income { get; set; }

        public decimal Outcome { get; set; }
        
        public DateTime Date { get; set; }
        
        public IEnumerable<BankAccountDetailOperationModel> Operations { get; set; }
    }

    public class BankAccountDetailOperationModel
    {
        public string Id { get; set; }

        public bool IsClosed { get; set; }

        public DateTime OperationDate { get; set; }

        public decimal Amount { get; set; }

        public bool IsTransfert { get; set; }

        public string OperationTypeId { get; set; }

        public string OperationTypeWording { get; set; }
    }
}