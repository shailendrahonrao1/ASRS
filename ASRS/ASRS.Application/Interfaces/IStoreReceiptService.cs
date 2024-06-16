using ASRS.Application.ServiceResponder;
using ASRS.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Application.Interfaces
{
    public interface IStoreReceiptService
    {
        Task<ServiceResponse<StoreReceiptResponse>> ImportStoreReceiptData(ImportAsrsRequest importFileRequest);
        Task<ServiceResponse<StoreReceiptResponse>> AddStoreReceiptData(StoreReceiptRequest storeReceiptRequest);
        Task<ServiceResponse<List<StoreReceiptResponse>>> GetAllStoreReceipts();
        Task<ServiceResponse<StoreReceiptResponse>> GetStoreReceiptByStoreReceiptNo(int storeReceiptNo);
        Task<ServiceResponse<StoreReceiptResponse>> GetStoreReceiptByLocationNo(string locationNo);
        Task<ServiceResponse<List<StoreReceiptResponse>>> GetStoreReceiptByTranscationStatus(string transcationStatus);
        Task<ServiceResponse<List<StoreReceiptResponse>>> GetStoreReceiptByMonthAndYearAsync(int month, int year);
        Task<ServiceResponse<StoreReceiptResponse>> UpdateStoreReceipt(StoreReceiptRequest storeReceiptRequest);
    }
}
