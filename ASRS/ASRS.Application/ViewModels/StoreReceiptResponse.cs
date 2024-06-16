using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.ViewModels
{
    public class StoreReceiptResponse
    {
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
        public string ItemCode { get; set; }

        public List<ItemMasterResponse> Items { get; set; } = new List<ItemMasterResponse>();
    }
}
