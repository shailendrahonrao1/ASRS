using ASRS.Core.Models;
using ASRS.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASRS.Infrastructure.Repositories
{
    public class ImportDataRepository : IImportDataRepository
    {
        private const string tableName = "StoreReceiptRequest";
        private readonly IOptions<ClientConfiguration> clientConfiguration;

        public ImportDataRepository(IOptions<ClientConfiguration> clientConfiguration)
        {
            this.clientConfiguration = clientConfiguration;
        }
        public async Task ImportStoreReceiptData(DataTable data)
        {
            var sqlConnection = clientConfiguration.Value.SqlConfiguration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                await InsertStoreReciptRequestData(data, connection);
            }
        }

        private static Task InsertStoreReciptRequestData(DataTable data, SqlConnection connection)
        {
            StoreReceiptRequest storeReceiptRequest = new StoreReceiptRequest();
            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
            {
                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.ColumnMappings.Add("CRVNo", storeReceiptRequest.CRVNo);
                sqlBulkCopy.ColumnMappings.Add("QuantityReceived", storeReceiptRequest.QuantityReceived.ToString());
                sqlBulkCopy.ColumnMappings.Add("ItemCode", storeReceiptRequest.ItemCode);
                sqlBulkCopy.ColumnMappings.Add("LedgerFolioNo", storeReceiptRequest.LedgerFolioNo);
                sqlBulkCopy.ColumnMappings.Add("ItemSerialNo", storeReceiptRequest.ItemSerialNo);
                sqlBulkCopy.ColumnMappings.Add("CRVDate", storeReceiptRequest.CRVDate.ToString());
                connection.Open();
                sqlBulkCopy.WriteToServer(data);
                connection.Close();
            }

            return Task.CompletedTask;
        }
    }
}
