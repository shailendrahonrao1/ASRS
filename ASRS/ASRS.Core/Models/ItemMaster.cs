﻿using System.ComponentModel.DataAnnotations;

namespace ASRS.Core.Models
{
    public class ItemMaster
    {
        [Key]
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string DenominationNo { get; set; }
        public string LedgerFolioNo { get; set; }
        public string ItemSerialNo { get; set; }
        public string ItemType { get; set; }
        public string ItemCategory { get; set; }
        public string Equipment { get; set; }

        public ICollection<StoreReceipt> StoreReceipts { get; } = new List<StoreReceipt>(); // Collection navigation containing dependents
        public ICollection<StockRelease> StockReleases { get; } = new List<StockRelease>(); // Collection navigation containing dependents
    }
}
