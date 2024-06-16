using ASRS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Infrastructure.Interfaces
{
    public interface IStoreReceiptRepository
    {
        Task<IEnumerable<StoreReceipt>> GetAllStoreReceiptsAsync();
        Task<StoreReceipt> GetStoreReceiptByStoreReceiptNoAsync(int storeReceiptNo);
        Task<StoreReceipt> GetStoreReceiptByLocationNoAsync(string locationNo);
        Task<IEnumerable<StoreReceipt>> GetStoreReceiptByTranscationStatusAsync(string transcationStatus);
        Task<IEnumerable<StoreReceipt>> GetStoreReceiptByMonthAndYearAsync(int month, int year);
        Task<StoreReceipt> UpdateStoreReceiptAsync(StoreReceipt storeReceipt);
    }
}
