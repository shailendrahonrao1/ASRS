using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.ViewModels
{
    public class StoreReceiptDataTable
    {
        public string CRVNo { get; set; }
        public int QuantityReceived { get; set; }
        public string ItemCode { get; set; }
        public string LedgerFolioNo { get; set; }
        public string ItemSerialNo { get; set; }
        public DateTime CRVDate { get; set; }
    }
}
