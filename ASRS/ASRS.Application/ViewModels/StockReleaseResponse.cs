using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.ViewModels
{
    public class StockReleaseResponse
    {
        public int StockReleaseNo { get; set; }
        public string DemandNo { get; set; }
        public string IssueVoucherNo { get; set; }
        public string DemandType { get; set; }
        public string ShipName { get; set; }
        public string SHNo { get; set; }
        public string Location { get; set; }
        public int DemandQuantity { get; set; }
        public int IssuedQuantity { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime DemandDate { get; set; }
        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime StockReleasedDate { get; set; }
        public List<ItemMasterResponse> Items { get; set; } = new List<ItemMasterResponse>();
    }
}
