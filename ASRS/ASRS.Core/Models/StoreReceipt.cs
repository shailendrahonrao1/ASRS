﻿using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

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

        public string ItemCode { get; set; } // Required foreign key property
        public ItemMaster ItemMaster { get; set; } = null!; // Required reference navigation to principal
    }
}
