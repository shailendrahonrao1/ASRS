using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.ViewModels
{
    public class ItemMasterRequest
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string DenominationNo { get; set; }
        public string LedgerFolioNo { get; set; }
        public string ItemSerialNo { get; set; }
        public string ItemType { get; set; }
        public string ItemCategory { get; set; }
        public string Equipment { get; set; }
    }
}
