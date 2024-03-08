using System.ComponentModel.DataAnnotations;

namespace ASRS.Core.Models
{
    public class StoreReceipt
    {
        [Key]
        public int StoreReceiptNo { get; set; }
        public string CRVNo { get; set; }
        public string SHNo { get; set; }
        public int QuantityReceived { get; set; }
        public int QuantityStored { get; set; }
        public string Location { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime CRVDate { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; } 
        public DateTime StoredDate { get; set; }
    }
}
