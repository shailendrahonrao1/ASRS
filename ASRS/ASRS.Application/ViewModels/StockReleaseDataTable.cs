using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.ViewModels
{
    public class StockReleaseDataTable
    {
        public int StockReleaseNo { get; set; }
        public string DemandNo { get; set; }
        public string ItemCode { get; set; }
        public string IssueVoucherNo { get; set; }
        public string DemandType { get; set; }
        public string ShipName { get; set; }
        public int DemandQuantity { get; set; }
        public DateTime DemandDate { get; set; }
    }
}
