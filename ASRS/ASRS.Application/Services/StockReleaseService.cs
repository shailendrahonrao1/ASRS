using ASRS.Infrastructure.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VM = ASRS.Application.ViewModels;
using DM = ASRS.Core.Models;
using System.Text;
using ASRS.Core.Models;
using ASRS.Application.ViewModels;
using ASRS.Application.Interfaces;
using ASRS.Application.ServiceResponder;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Data.OleDb;
using System.Data;

namespace ASRS.Application.Services
{
    public class StockReleaseService : IStockReleaseService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOptions<ClientConfiguration> _clientConfiguration;
        private readonly IImportDataRepository _importDataRepository;
        private readonly AbstractValidator<ImportAsrsRequest> _importAsrsRequestValidator;
        private readonly ILogger<StoreReceiptService> _logger;
        private readonly IMapper _mapper;
        private readonly IStockReleaseRepository _stockReleaseRepository;
        private readonly AbstractValidator<StockReleaseRequest> _stockReleaseRequestValidator;

        public StockReleaseService(IWebHostEnvironment webHostEnvironment,
            IOptions<DM.ClientConfiguration> clientConfiguration,
            IImportDataRepository importDataRepository,
            AbstractValidator<VM.ImportAsrsRequest> importAsrsRequestValidator,
            ILogger<StoreReceiptService> logger,
            IMapper mapper,
            IStockReleaseRepository stockReleaseRepository,
            AbstractValidator<StockReleaseRequest> stockReleaseRequestValidator)
             
        {
            _webHostEnvironment = webHostEnvironment;
            _clientConfiguration = clientConfiguration;
            _importDataRepository = importDataRepository;
            _importAsrsRequestValidator = importAsrsRequestValidator;
            _logger = logger;
            _mapper = mapper;
            _stockReleaseRepository = stockReleaseRepository;
            _stockReleaseRequestValidator = stockReleaseRequestValidator;
        }

        public async Task<ServiceResponse<List<StockReleaseResponse>>> GetAllStockRelease()
        {
            var stockReleaseResponse = new List<StockReleaseResponse>();
            var stockReleases = await _stockReleaseRepository.GetAllStockReleasesAsync().ConfigureAwait(false);
            if (!stockReleases.Any())
            {
                return new ServiceResponse<List<StockReleaseResponse>>
                {
                    Success = false,
                };
            }
            foreach (var stockRelease in stockReleases)
            {
                stockReleaseResponse.Add(_mapper.Map<StockReleaseResponse>(stockRelease));
            }
            return new ServiceResponse<List<StockReleaseResponse>>
            {
                Success = true,
                Data = stockReleaseResponse
            };
        }

        public async Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByLocationNo(string locationNo)
        {
            var stockRelease = await _stockReleaseRepository.GetStockReleaseByLocationNoAsync(locationNo).ConfigureAwait(false);
            if (stockRelease == null)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StockReleaseResponse>
            {
                Success = true,
                Data = _mapper.Map<StockReleaseResponse>(stockRelease),
            };
        }

        public async Task<ServiceResponse<List<StockReleaseResponse>>> GetStockReleaseByMonthAndYear(int month, int year)
        {
            var stockReleaseResponse = new List<StockReleaseResponse>();
            var stockReleases = await _stockReleaseRepository.GetStockReleaseByMonthAndYearAsync(month, year).ConfigureAwait(false);
            if (stockReleases == null)
            {
                return new ServiceResponse<List<StockReleaseResponse>>
                {
                    Success = false
                };
            }
            foreach (var stockRelease in stockReleases)
            {
                stockReleaseResponse.Add(_mapper.Map<StockReleaseResponse>(stockRelease));
            }
            return new ServiceResponse<List<StockReleaseResponse>>
            {
                Success = true,
                Data = stockReleaseResponse
            };
        }

        public async Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByStockReleaseNo(int stockReleaseNo)
        {
            var stockRelease = await _stockReleaseRepository.GetStockReleaseByStockReleaseNoAsync(stockReleaseNo).ConfigureAwait(false);
            if (stockRelease == null)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StockReleaseResponse>
            {
                Success = true,
                Data = _mapper.Map<StockReleaseResponse>(stockRelease),
            };
        }

        public async Task<ServiceResponse<List<StockReleaseResponse>>> GetStockReleaseByTranscationStatus(string transcationStatus)
        {
            var stockReleaseResponse = new List<StockReleaseResponse>();
            var stockReleases = await _stockReleaseRepository.GetStockReleaseByTranscationStatusAsync(transcationStatus).ConfigureAwait(false);
            if (stockReleases == null)
            {
                return new ServiceResponse<List<StockReleaseResponse>>
                {
                    Success = false
                };
            }
            foreach (var stockRelease in stockReleases)
            {
                stockReleaseResponse.Add(_mapper.Map<StockReleaseResponse>(stockRelease));
            }
            return new ServiceResponse<List<StockReleaseResponse>>
            {
                Success = true,
                Data = stockReleaseResponse
            };
        }

        public async Task<ServiceResponse<StockReleaseResponse>> ImportStockReleaseData(ImportAsrsRequest importFileRequest)
        {
            var stockRelease = new DM.StockRelease();
            _logger.LogInformation("Validating StockRelease excel file");
            var result = _importAsrsRequestValidator.Validate(importFileRequest);
            if (!result.IsValid)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false,
                    Error = result.Errors.FirstOrDefault().ErrorMessage
                };
            }
            var path = GetUplodedFilePath(importFileRequest.file);

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    _logger.LogInformation("Importing stock release excel data");
                    stockRelease = await ImportStockReleaseData(stockRelease, reader).ConfigureAwait(false);
                }
            }
            if (stockRelease == null)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false
                };
            }
            return new ServiceResponse<StockReleaseResponse>
            {
                Success = true,
                Data = _mapper.Map<StockReleaseResponse>(stockRelease),
                Message = "File uploaded successfully"
            };
        }

        public async Task<ServiceResponse<StockReleaseResponse>> UpdateStockRelease(StockReleaseRequest stockReleaseRequest)
        {
            var result = _stockReleaseRequestValidator.Validate(stockReleaseRequest);
            if (!result.IsValid)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false,
                    Error = result.Errors.FirstOrDefault().ErrorMessage
                };
            }

            // convert ViewModel to Domain Model
            var stockRelease = _mapper.Map<DM.StockRelease>(stockReleaseRequest);

            stockRelease = await _stockReleaseRepository.UpdateStockReleaseAsync(stockRelease).ConfigureAwait(false);
            if (stockRelease == null)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StockReleaseResponse>
            {
                Success = true,
                Data = _mapper.Map<StockReleaseResponse>(stockRelease),
            };
        }

        public async Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByDemandNo(string demandNo)
        {
            var stockRelease = await _stockReleaseRepository.GetStockReleaseByDemandNoAsync(demandNo).ConfigureAwait(false);
            if (stockRelease == null)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StockReleaseResponse>
            {
                Success = true,
                Data = _mapper.Map<StockReleaseResponse>(stockRelease),
            };
        }

        public async Task<ServiceResponse<StockReleaseResponse>> GetStockReleaseByIssueVoucherNo(string issueVoucherNo)
        {
            var stockRelease = await _stockReleaseRepository.GetStockReleaseByIssueVoucherNoAsync(issueVoucherNo).ConfigureAwait(false);
            if (stockRelease == null)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StockReleaseResponse>
            {
                Success = true,
                Data = _mapper.Map<StockReleaseResponse>(stockRelease),
            };
        }

        public async Task<ServiceResponse<StockReleaseResponse>> AddStockReleaseData(StockReleaseRequest stockReleaseRequest)
        {
            _logger.LogInformation("Validating StockRelease request");
            var result = _stockReleaseRequestValidator.Validate(stockReleaseRequest);
            if (!result.IsValid)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false,
                    Error = result.Errors.FirstOrDefault().ErrorMessage
                };
            }

            // convert ViewModel to Domain Model
            var itemMaster = new DM.ItemMaster()
            {
                ItemCode = stockReleaseRequest.ItemCode.ToString(),
            };

            var stockRelease = new DM.StockRelease()
            {
                ItemMaster = itemMaster,
                DemandNo = stockReleaseRequest.DemandNo.ToString(),
                ItemCode = stockReleaseRequest.ItemCode.ToString(),
                IssueVoucherNo = stockReleaseRequest.IssueVoucherNo.ToString(),
                DemandType = stockReleaseRequest.DemandType.ToString(),
                ShipName = stockReleaseRequest.ShipName.ToString(),
                DemandQuantity = Convert.ToInt32(stockReleaseRequest.DemandQuantity.ToString()),
                DemandDate = Convert.ToDateTime(stockReleaseRequest.DemandDate.ToString())
            };

            _logger.LogInformation("Inserting stock release data");

            stockRelease = await _importDataRepository.ImportStockReleaseDataAsync(stockRelease).ConfigureAwait(false);

            if (stockRelease == null)
            {
                return new ServiceResponse<StockReleaseResponse>
                {
                    Success = false
                };
            }

            _logger.LogInformation("Inserted stock release data");

            return new ServiceResponse<StockReleaseResponse>
            {
                Success = true,
                Data = _mapper.Map<StockReleaseResponse>(stockRelease),
                Message = "Stock release data inserted successfully"
            };
        }

        #region Private Methods
        private async Task<StockRelease> ImportStockReleaseData(StockRelease stockRelease, IExcelDataReader reader)
        {
            List<DataTable> dataTables = ExcelToDataTable(reader);
            DataTable dt = dataTables[0];
            var importStockReleaseRequests = ConvertDataTable<VM.StockReleaseDataTable>(dt);
            foreach (var item in importStockReleaseRequests)
            {
                var itemMaster = new DM.ItemMaster()
                {
                    ItemCode = item.ItemCode.ToString(),
                };

                stockRelease = new DM.StockRelease()
                {
                    ItemMaster = itemMaster,
                    DemandNo = item.DemandNo.ToString(),
                    ItemCode = item.ItemCode.ToString(),
                    IssueVoucherNo = item.IssueVoucherNo.ToString(),
                    DemandType = item.DemandType.ToString(),
                    ShipName = item.ShipName.ToString(),
                    DemandQuantity = Convert.ToInt32(item.DemandQuantity.ToString()),
                    DemandDate = Convert.ToDateTime(item.DemandDate.ToString())
                };

                stockRelease = await _importDataRepository.ImportStockReleaseDataAsync(stockRelease).ConfigureAwait(false);
                _logger.LogInformation("Successfully imported StockRelease excel file");
            }

            return stockRelease;
        }
        private static List<DataTable> ExcelToDataTable(IExcelDataReader reader)
        {
            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (data) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            List<DataTable> dataTables = new List<DataTable>();
            dataTables = new List<DataTable>(result.Tables.Cast<DataTable>());
            return dataTables;
        }
        private string GetUplodedFilePath(IFormFile formFile)
        {
            var localPath = Path.Combine(_webHostEnvironment.ContentRootPath, "ImportFiles", $"{formFile.FileName}");
            using (var stream = new FileStream(localPath, FileMode.Create))
            {
                formFile.CopyToAsync(stream);
            }

            return localPath;
        }
        private DataTable StoreReceiptDataTable(string path)
        {
            DataTable dataTable = new DataTable();
            var excelConnectionString = _clientConfiguration.Value.ExcelConfiguration.ConnectionString;
            excelConnectionString = string.Format(excelConnectionString, path);
            try
            {
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
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }

            return dataTable;
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            //Type temp = typeof(T);
            //T obj = Activator.CreateInstance<T>();
            var list = new List<T>();
            var boolTypes = new List<Type>() { typeof(bool?), typeof(bool) };
            var boolValues = new List<string>() { "true", "yes", "1", "y" };
            var stringTypes = new List<Type>() { typeof(string) };
            var dateTypes = new List<Type>() { typeof(DateTime?), typeof(DateTime) };
            var intTypes = new List<Type>() { typeof(int) };

            //foreach (DataColumn column in dr.Table.Columns)
            //{
            //    foreach (PropertyInfo pro in temp.GetProperties())
            //    {
            //        if (pro.Name == column.ColumnName)
            //            pro.SetValue(obj, dr[column.ColumnName], null);
            //        else
            //            continue;
            //    }
            //}
            //return obj;
            var entityModel = (T)Activator.CreateInstance(typeof(T));
            foreach (var property in entityModel.GetType().GetProperties())
            {
                var propValue = dr[property.Name.ToLower()].ToString();

                object value = null;
                var memType = property.PropertyType;

                if (boolTypes.Contains(memType))
                    value = boolValues.Contains(propValue.ToLower());
                else if (stringTypes.Contains(memType))
                    value = string.IsNullOrWhiteSpace(propValue) ? null : propValue.Trim();
                else if (dateTypes.Contains(memType))
                    value = string.IsNullOrWhiteSpace(propValue) ? (DateTime?)null : DateTime.Parse(propValue);
                else if (intTypes.Contains(memType))
                    value = string.IsNullOrWhiteSpace(propValue) ? 0 : int.Parse(propValue);

                entityModel.GetType().GetProperty(property.Name).SetValue(entityModel, value);
            }

            return entityModel;
        }

        #endregion
    }
}
