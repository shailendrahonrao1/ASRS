using ASRS.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Infrastructure.Interfaces
{
    public interface IImportDataRepository
    {
        Task ImportStoreReceiptData(DataTable data);
        Task<StoreReceipt> ImportStoreReceiptDataAsync(StoreReceipt storeReceipt);
        Task<StockRelease> ImportStockReleaseDataAsync(StockRelease stockRelease);
    }
}
