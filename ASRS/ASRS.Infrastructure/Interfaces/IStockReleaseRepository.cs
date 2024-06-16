using ASRS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Infrastructure.Interfaces
{
    public interface IStockReleaseRepository
    {
        Task<IEnumerable<StockRelease>> GetAllStockReleasesAsync();
        Task<StockRelease> GetStockReleaseByStockReleaseNoAsync(int stckReleaseNo);
        Task<StockRelease> GetStockReleaseByDemandNoAsync(string demandNo);
        Task<StockRelease> GetStockReleaseByIssueVoucherNoAsync(string issueVoucherNo);
        Task<StockRelease> GetStockReleaseByLocationNoAsync(string locationNo);
        Task<IEnumerable<StockRelease>> GetStockReleaseByTranscationStatusAsync(string transcationStatus);
        Task<IEnumerable<StockRelease>> GetStockReleaseByMonthAndYearAsync(int month, int year);
        Task<StockRelease> UpdateStockReleaseAsync(StockRelease stockRelease);
    }
}
