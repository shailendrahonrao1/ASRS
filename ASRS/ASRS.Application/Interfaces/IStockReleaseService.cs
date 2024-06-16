using ASRS.Application.ServiceResponder;
using ASRS.Application.ViewModels;

namespace ASRS.Application.Interfaces
{
    public interface IStockReleaseService
    {
        Task<ServiceResponse<StockReleaseResponse>> ImportStockReleaseData(ImportAsrsRequest importFileRequest);
        Task<ServiceResponse<StockReleaseResponse>> AddStockReleaseData(StockReleaseRequest stockReleaseRequest);
        Task<ServiceResponse<List<StockReleaseResponse>>> GetAllStockRelease();
        Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByStockReleaseNo(int stockReleaseNo);
        Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByDemandNo(string demandNo);
        Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByIssueVoucherNo(string issueVoucherNo);
        Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByLocationNo(string locationNo);
        Task<ServiceResponse<List<StockReleaseResponse>>> GetStockReleaseByTranscationStatus(string transcationStatus);
        Task<ServiceResponse<List<StockReleaseResponse>>> GetStockReleaseByMonthAndYear(int month, int year);
        Task<ServiceResponse<StockReleaseResponse>> UpdateStockRelease(StockReleaseRequest stockReleaseRequest);
    }
}
