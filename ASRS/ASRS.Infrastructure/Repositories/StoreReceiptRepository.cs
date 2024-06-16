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

namespace ASRS.Infrastructure.Repositories
{
    public class StoreReceiptRepository : IStoreReceiptRepository
    {
        private readonly IOptions<ClientConfiguration> _clientConfiguration;
        private readonly ILogger<ImportDataRepository> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public StoreReceiptRepository(IOptions<ClientConfiguration> clientConfiguration,
            ILogger<ImportDataRepository> logger,
            ApplicationDbContext applicationDbContext)
        {
            _clientConfiguration = clientConfiguration;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<StoreReceipt>> GetAllStoreReceiptsAsync()
        {
            return await _applicationDbContext.StoreReceipts.ToListAsync().ConfigureAwait(false);
        }

        public async Task<StoreReceipt> GetStoreReceiptByLocationNoAsync(string locationNo)
        {
            return await _applicationDbContext.StoreReceipts.FirstOrDefaultAsync(x => x.Location == locationNo).ConfigureAwait(false);
        }

        public async Task<IEnumerable<StoreReceipt>> GetStoreReceiptByMonthAndYearAsync(int month, int year)
        {
            return await _applicationDbContext.StoreReceipts.Where(x => x.CRVDate.Month == month && x.CRVDate.Year == year).ToListAsync();
        }

        public async Task<StoreReceipt> GetStoreReceiptByStoreReceiptNoAsync(int storeReceiptNo)
        {
            return await _applicationDbContext.StoreReceipts.FirstOrDefaultAsync(x => x.StoreReceiptNo == storeReceiptNo).ConfigureAwait(false);
        }

        public async Task<IEnumerable<StoreReceipt>> GetStoreReceiptByTranscationStatusAsync(string transcationStatus)
        {
            return await _applicationDbContext.StoreReceipts.Where(x => x.TransactionStatus.Contains(transcationStatus)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<StoreReceipt> UpdateStoreReceiptAsync(StoreReceipt storeReceipt)
        {
            var existingStoreReceipt = await _applicationDbContext.StoreReceipts.FirstOrDefaultAsync(x => x.StoreReceiptNo == storeReceipt.StoreReceiptNo).ConfigureAwait(false);
            if (existingStoreReceipt != null)
            {
                _applicationDbContext.Entry(existingStoreReceipt).CurrentValues.SetValues(storeReceipt);
                await _applicationDbContext.SaveChangesAsync().ConfigureAwait(false);

                return storeReceipt;
            }
            return null;
        }
    }
}
