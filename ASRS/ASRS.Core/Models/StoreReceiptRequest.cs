using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Core.Models
{
    public class StoreReceiptRequest
    {
        [Key]
        public int Id { get; set; }
        public string CRVNo { get; set; }
        public int QuantityReceived { get; set; }
        public string ItemCode { get; set; }
        public string LedgerFolioNo { get; set; }
        public string ItemSerialNo { get; set; }
        public DateTime CRVDate { get; set; }
        public string CreatedBy { get; set; }
    }
}
