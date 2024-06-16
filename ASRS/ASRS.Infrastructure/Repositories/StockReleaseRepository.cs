using ASRS.Core.Models;
using ASRS.Infrastructure.Data;
using ASRS.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ASRS.Infrastructure.Repositories
{
    public class StockReleaseRepository : IStockReleaseRepository
    {
        private readonly IOptions<ClientConfiguration> _clientConfiguration;
        private readonly ILogger<ImportDataRepository> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public StockReleaseRepository(IOptions<ClientConfiguration> clientConfiguration,
            ILogger<ImportDataRepository> logger,
            ApplicationDbContext applicationDbContext)
        {
            _clientConfiguration = clientConfiguration;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<StockRelease>> GetAllStockReleasesAsync()
        {
            return await _applicationDbContext.StockReleases.ToListAsync().ConfigureAwait(false);
        }

        public async Task<StockRelease> GetStockReleaseByDemandNoAsync(string demandNo)
        {
            return await _applicationDbContext.StockReleases.FirstOrDefaultAsync(x => x.DemandNo == demandNo).ConfigureAwait(false);
        }

        public async Task<StockRelease> GetStockReleaseByIssueVoucherNoAsync(string issueVoucherNo)
        {
            return await _applicationDbContext.StockReleases.FirstOrDefaultAsync(x => x.IssueVoucherNo == issueVoucherNo).ConfigureAwait(false);
        }

        public async Task<StockRelease> GetStockReleaseByLocationNoAsync(string locationNo)
        {
            return await _applicationDbContext.StockReleases.FirstOrDefaultAsync(x => x.Location == locationNo).ConfigureAwait(false);
        }

        public async Task<IEnumerable<StockRelease>> GetStockReleaseByMonthAndYearAsync(int month, int year)
        {
            return await _applicationDbContext.StockReleases.Where(x => x.DemandDate.Month == month && x.DemandDate.Year == year).ToListAsync().ConfigureAwait(false);
        }

        public async Task<StockRelease> GetStockReleaseByStockReleaseNoAsync(int stockReleaseNo)
        {
            return await _applicationDbContext.StockReleases.FirstOrDefaultAsync(x => x.StockReleaseNo == stockReleaseNo).ConfigureAwait(false);
        }

        public async Task<IEnumerable<StockRelease>> GetStockReleaseByTranscationStatusAsync(string transcationStatus)
        {
            return await _applicationDbContext.StockReleases.Where(x => x.TransactionStatus.Contains(transcationStatus)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<StockRelease> UpdateStockReleaseAsync(StockRelease stockRelease)
        {
            var existingStockRelease = await _applicationDbContext.StockReleases.FirstOrDefaultAsync(x => x.StockReleaseNo == stockRelease.StockReleaseNo).ConfigureAwait(false);
            if (existingStockRelease != null)
            {
                _applicationDbContext.Entry(existingStockRelease).CurrentValues.SetValues(stockRelease);
                await _applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

                return stockRelease;
            }
            return null;
        }
    }
}
