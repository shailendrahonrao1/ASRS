using ASRS.Core.Models;
using ASRS.Infrastructure.Data;
using ASRS.Infrastructure.Interfaces;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Infrastructure.Repositories
{
    public class ImportDataRepository : IImportDataRepository
    {
        private const string tableName = "dbo.StoreReceipts";
        private readonly IOptions<ClientConfiguration> _clientConfiguration;
        private readonly ILogger<ImportDataRepository> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public ImportDataRepository(IOptions<ClientConfiguration> clientConfiguration, 
            ILogger<ImportDataRepository> logger,
            ApplicationDbContext applicationDbContext)
        {
            _clientConfiguration = clientConfiguration;
            _logger = logger;
            _applicationDbContext = applicationDbContext;
        }
        public async Task ImportStoreReceiptData(DataTable data)
        {
            var sqlConnection = _clientConfiguration.Value.SqlConfiguration.ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnection))
                {
                    await InsertStoreReciptRequestData(data, connection);
                }
            }
            catch (Exception ex) 
            {
                _logger.LogInformation(ex.Message);
            }
        }

        public async Task<StoreReceipt> ImportStoreReceiptDataAsync(StoreReceipt storeReceipt)
        {
            await _applicationDbContext.AddAsync(storeReceipt).ConfigureAwait(false);
            var result = await _applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
            if (result <= 0) return null;
            return storeReceipt;
        }

        public async Task<StockRelease> ImportStockReleaseDataAsync(StockRelease stockRelease)
        {
            await _applicationDbContext.AddAsync(stockRelease).ConfigureAwait(false);
            var result = await _applicationDbContext.SaveChangesAsync().ConfigureAwait (false);
            if (result <= 0) return null; 
            return stockRelease;
        }

        #region Private Methods
        private static Task InsertStoreReciptRequestData(DataTable data, SqlConnection connection)
        {
            ImportStoreReceiptRequest storeReceiptRequest = new ImportStoreReceiptRequest();
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
            {
                sqlBulkCopy.DestinationTableName = tableName;
                //sqlBulkCopy.ColumnMappings.Add("CRVNo", storeReceiptRequest.CRVNo.ToString());
                //sqlBulkCopy.ColumnMappings.Add("QuantityReceived", storeReceiptRequest.QuantityReceived.ToString());
                //sqlBulkCopy.ColumnMappings.Add("ItemCode", storeReceiptRequest.ItemCode.ToString());
                //sqlBulkCopy.ColumnMappings.Add("LedgerFolioNo", storeReceiptRequest.LedgerFolioNo.ToString());
                //sqlBulkCopy.ColumnMappings.Add("ItemSerialNo", storeReceiptRequest.ItemSerialNo.ToString());
                //sqlBulkCopy.ColumnMappings.Add("CRVDate", storeReceiptRequest.CRVDate.ToString());
                foreach (DataColumn col in data.Columns)
                {
                    sqlBulkCopy.ColumnMappings.Add(col.ColumnName.ToLower(), col.ColumnName.ToLower());
                }
                connection.Open();
                sqlBulkCopy.WriteToServer(data);
                connection.Close();
            }



            return Task.CompletedTask;
        }

        #endregion

    }
}
