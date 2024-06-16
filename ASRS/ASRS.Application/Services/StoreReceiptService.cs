using ASRS.Application.Interfaces;
using VM = ASRS.Application.ViewModels;
using DM = ASRS.Core.Models;
using ASRS.Infrastructure.Interfaces;
using ExcelDataReader;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.OleDb;
using System.Text;
using AutoMapper;
using ASRS.Application.ViewModels;
using ASRS.Core.Models;
using ASRS.Application.ServiceResponder;
using System.Reflection.PortableExecutable;

namespace ASRS.Application.Services
{
    public class StoreReceiptService : IStoreReceiptService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IOptions<DM.ClientConfiguration> _clientConfiguration;
        private readonly IImportDataRepository _importDataRepository;
        private readonly AbstractValidator<VM.ImportAsrsRequest> _importAsrsRequestValidator;
        private readonly ILogger<StoreReceiptService> _logger;
        private readonly IMapper _mapper;
        private readonly IStoreReceiptRepository _storeReceiptRepository;
        private readonly AbstractValidator<StoreReceiptRequest> _storeReceiptRequestValidator;

        public StoreReceiptService(IWebHostEnvironment webHostEnvironment,
            IOptions<DM.ClientConfiguration> clientConfiguration,
            IImportDataRepository importDataRepository,
            AbstractValidator<VM.ImportAsrsRequest> importAsrsRequestValidator,
            ILogger<StoreReceiptService> logger,
            IMapper mapper,
            IStoreReceiptRepository storeReceiptRepository,
            AbstractValidator<VM.StoreReceiptRequest> storeReceiptRequestValidator)
        {
            _webHostEnvironment = webHostEnvironment;
            _clientConfiguration = clientConfiguration;
            _importDataRepository = importDataRepository;
            _importAsrsRequestValidator = importAsrsRequestValidator;
            _logger = logger;
            _mapper = mapper;
            _storeReceiptRepository = storeReceiptRepository;
            _storeReceiptRequestValidator = storeReceiptRequestValidator;
        }
        public async Task<ServiceResponse<StoreReceiptResponse>> ImportStoreReceiptData(VM.ImportAsrsRequest importAsrsRequest)
        {
            var storeReceipt = new DM.StoreReceipt();
            _logger.LogInformation("Validating StoreReceipt excel file");
            var result = _importAsrsRequestValidator.Validate(importAsrsRequest);
            if (!result.IsValid)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false,
                    Error = result.Errors.FirstOrDefault().ErrorMessage
                };
            }
            var path = GetUplodedFilePath(importAsrsRequest.file);
            
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    _logger.LogInformation("Importing store receipt excel data");
                    storeReceipt = await ImportStoreReceiptData(storeReceipt, reader).ConfigureAwait(false);

                }
            }
            if (storeReceipt== null)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false
                };
            }
            return new ServiceResponse<StoreReceiptResponse>
            {
                Success = true,
                Data = _mapper.Map<StoreReceiptResponse>(storeReceipt),
                Message = "File uploaded successfully"
            };
        }
        public async Task<ServiceResponse<List<StoreReceiptResponse>>> GetAllStoreReceipts()
        {
            var storeReceiptResponse = new List<StoreReceiptResponse>();
            var storeReceipts = await _storeReceiptRepository.GetAllStoreReceiptsAsync().ConfigureAwait(false);
            if (!storeReceipts.Any())
            {
                return new ServiceResponse<List<StoreReceiptResponse>>
                {
                    Success = false,
                };
            }
            foreach (var storeReceipt in storeReceipts)
            {
                storeReceiptResponse.Add(_mapper.Map<StoreReceiptResponse>(storeReceipt));
            }
            return new ServiceResponse<List<StoreReceiptResponse>>
            {
                Success = true,
                Data = storeReceiptResponse
            };
        }
        public async Task<ServiceResponse<StoreReceiptResponse>> GetStoreReceiptByLocationNo(string locationNo)
        {
            var storeReceipt = await _storeReceiptRepository.GetStoreReceiptByLocationNoAsync(locationNo).ConfigureAwait(false);
            if (storeReceipt == null)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StoreReceiptResponse>
            {
                Success = true,
                Data = _mapper.Map<StoreReceiptResponse>(storeReceipt),
            };
        }
        public async Task<ServiceResponse<StoreReceiptResponse>> GetStoreReceiptByStoreReceiptNo(int storeReceiptNo)
        {
            var storeReceipt = await _storeReceiptRepository.GetStoreReceiptByStoreReceiptNoAsync(storeReceiptNo).ConfigureAwait(false);
            if (storeReceipt == null)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StoreReceiptResponse>
            {
                Success = true,
                Data = _mapper.Map<StoreReceiptResponse>(storeReceipt),
            };
        }
        public async Task<ServiceResponse<List<StoreReceiptResponse>>> GetStoreReceiptByMonthAndYearAsync(int month, int year)
        {
            var storeReceiptResponse = new List<StoreReceiptResponse>();
            var storeReceipts = await _storeReceiptRepository.GetStoreReceiptByMonthAndYearAsync(month, year).ConfigureAwait(false);
            if (storeReceipts == null)
            {
                return new ServiceResponse<List<StoreReceiptResponse>>
                {
                    Success = false
                };
            }
            foreach (var storeReceipt in storeReceipts)
            {
                storeReceiptResponse.Add(_mapper.Map<StoreReceiptResponse>(storeReceipt));
            }
            return new ServiceResponse<List<StoreReceiptResponse>>
            {
                Success = true,
                Data = storeReceiptResponse
            };
        }
        public async Task<ServiceResponse<List<StoreReceiptResponse>>> GetStoreReceiptByTranscationStatus(string transcationStatus)
        {
            var storeReceiptResponse = new List<StoreReceiptResponse>();
            var storeReceipts = await _storeReceiptRepository.GetStoreReceiptByTranscationStatusAsync(transcationStatus).ConfigureAwait(false);
            if (storeReceipts == null)
            {
                return new ServiceResponse<List<StoreReceiptResponse>>
                {
                    Success = false
                };
            }
            foreach (var storeReceipt in storeReceipts)
            {
                storeReceiptResponse.Add(_mapper.Map<StoreReceiptResponse>(storeReceipt));
            }
            return new ServiceResponse<List<StoreReceiptResponse>>
            {
                Success = true,
                Data = storeReceiptResponse
            };
        }
        public async Task<ServiceResponse<StoreReceiptResponse>> UpdateStoreReceipt(StoreReceiptRequest storeReceiptRequest)
        {
            var result = _storeReceiptRequestValidator.Validate(storeReceiptRequest);
            if (!result.IsValid)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false,
                    Error = result.Errors.FirstOrDefault().ErrorMessage
                };
            }

            // convert ViewModel to Domain Model
            var storeReceipt = _mapper.Map<DM.StoreReceipt>(storeReceiptRequest);

            storeReceipt = await _storeReceiptRepository.UpdateStoreReceiptAsync(storeReceipt).ConfigureAwait(false);
            if (storeReceipt == null)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false
                };
            }

            return new ServiceResponse<StoreReceiptResponse>
            {
                Success = true,
                Data = _mapper.Map<StoreReceiptResponse>(storeReceipt),
            };
        }
        public async Task<ServiceResponse<StoreReceiptResponse>> AddStoreReceiptData(StoreReceiptRequest storeReceiptRequest)
        {
            _logger.LogInformation("Validating StoreReceipt request");
            var result = _storeReceiptRequestValidator.Validate(storeReceiptRequest);
            if (!result.IsValid)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false,
                    Error = result.Errors.FirstOrDefault().ErrorMessage
                };
            }

            // convert ViewModel to Domain Model
            
            var itemMaster = new DM.ItemMaster()
            {
                ItemCode = storeReceiptRequest.ItemCode.ToString(),
                LedgerFolioNo = storeReceiptRequest.LedgerFolioNo.ToString(),
                ItemSerialNo = storeReceiptRequest.ItemSerialNo.ToString()
            };

            var storeReceipt = new DM.StoreReceipt()
            {
                ItemMaster = itemMaster,
                CRVNo = storeReceiptRequest.CRVNo.ToString(),
                ItemCode = storeReceiptRequest.ItemCode.ToString(),
                QuantityReceived = Convert.ToInt32(storeReceiptRequest.QuantityReceived.ToString()),
                CRVDate = Convert.ToDateTime(storeReceiptRequest.CRVDate.ToString())
            };

            _logger.LogInformation("Inserting store receipt data");
            storeReceipt = await _importDataRepository.ImportStoreReceiptDataAsync(storeReceipt).ConfigureAwait(false);
            if (storeReceipt == null)
            {
                return new ServiceResponse<StoreReceiptResponse>
                {
                    Success = false
                };
            }

            _logger.LogInformation("Inserted store receipt data");

            return new ServiceResponse<StoreReceiptResponse>
            {
                Success = true,
                Data = _mapper.Map<StoreReceiptResponse>(storeReceipt),
                Message = "Store receipt data inserted successfully"
            };
        }

        #region Private Methods
        private async Task<StoreReceipt> ImportStoreReceiptData(StoreReceipt storeReceipt, IExcelDataReader reader)
        {
            List<DataTable> dataTables = ExcelToDataTable(reader);
            DataTable dt = dataTables[0];
            var importStoreReceiptRequests = ConvertDataTable<VM.StoreReceiptDataTable>(dt);

            foreach (var item in importStoreReceiptRequests)
            {
                var itemMaster = new DM.ItemMaster()
                {
                    ItemCode = item.ItemCode.ToString(),
                    LedgerFolioNo = item.LedgerFolioNo.ToString(),
                    ItemSerialNo = item.ItemSerialNo.ToString()
                };

                storeReceipt = new DM.StoreReceipt()
                {
                    ItemMaster = itemMaster,
                    CRVNo = item.CRVNo.ToString(),
                    ItemCode = item.ItemCode.ToString(),
                    QuantityReceived = Convert.ToInt32(item.QuantityReceived.ToString()),
                    CRVDate = Convert.ToDateTime(item.CRVDate.ToString())
                };
                storeReceipt = await _importDataRepository.ImportStoreReceiptDataAsync(storeReceipt).ConfigureAwait(false);
                _logger.LogInformation("Imported StoreReceipt excel file");
            }

            return storeReceipt;
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
