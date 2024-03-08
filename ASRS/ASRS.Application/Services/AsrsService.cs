using ASRS.Application.Interfaces;
using ASRS.Application.ViewModels;
using ASRS.Core.Models;
using ASRS.Infrastructure.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASRS.Application.Services
{
    public class AsrsService : IAsrsService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IOptions<ClientConfiguration> clientConfiguration;
        private readonly IImportDataRepository importDataRepository;
        private readonly AbstractValidator<ImportAsrsRequest> validator;

        public AsrsService(IWebHostEnvironment webHostEnvironment,
            IOptions<ClientConfiguration> clientConfiguration,
            IImportDataRepository importDataRepository,
            AbstractValidator<ImportAsrsRequest> validator)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.clientConfiguration = clientConfiguration;
            this.importDataRepository = importDataRepository;
            this.validator = validator;
        }
        public async Task<(bool flag, string message)> ImportStoreReceiptData(ImportAsrsRequest importAsrsRequest)
        {
            var result = validator.Validate(importAsrsRequest);
            if(!result.IsValid)
            {
                return (false, result.Errors.FirstOrDefault().ErrorMessage);
            }
            var path = GetUplodedFilePath(importAsrsRequest.file);
            DataTable storeReceiptDataTable = StoreReceiptDataTable(path);
            await importDataRepository.ImportStoreReceiptData(storeReceiptDataTable);
            return (true, "File imported successfully");
        }

        private string GetUplodedFilePath(IFormFile formFile)
        {
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "ImportFiles", $"{formFile.FileName}");
            using (var stream = new FileStream(localPath, FileMode.Create)) 
            {
                formFile.CopyToAsync(stream);
            }

            return localPath;
        }
        private DataTable StoreReceiptDataTable(string path)
        {
            DataTable dataTable = new DataTable();
            var excelConnectionString = clientConfiguration.Value.ExcelConfiguration.ConnectionString;
            excelConnectionString = string.Format(excelConnectionString, path);
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
            {
                using (OleDbCommand command = excelConnection.CreateCommand())
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        //Get the name of First Sheet
                        excelConnection.Open();
                        command.Connection = excelConnection;
                        DataTable excelSchema;
                        excelSchema = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();
                        excelConnection.Close();

                        // Read Data from First Sheet
                        excelConnection.Open();
                        command.CommandText = "SELECT * From [" + sheetName + "]";
                        adapter.SelectCommand = command;
                        adapter.Fill(dataTable);
                        excelConnection.Close();
                    }
                }
            }

            return dataTable;
        }
    }
}
