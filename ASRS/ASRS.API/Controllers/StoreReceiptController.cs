using ASRS.Application.Interfaces;
using ASRS.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASRS.API.Controllers
{
    [Route("api/asrs/v1/[controller]")]
    [ApiController]
    public class StoreReceiptController : ControllerBase
    {
        private readonly IStoreReceiptService _storeReceiptService;
        private readonly ILogger<StoreReceiptController> _logger;

        public StoreReceiptController(IStoreReceiptService storeReceiptService, ILogger<StoreReceiptController> logger)
        {
            _storeReceiptService = storeReceiptService;
            _logger = logger;
        }

        
        [HttpPost]
        [Authorize]
        [Route("ImportStoreReceiptData")]
        public async Task<IActionResult> ImportStoreReceiptRequestData(ImportAsrsRequest importAsrsRequest)
        {
            _logger.LogInformation("Importing StoreReceipt excel data");
            var result = await _storeReceiptService.ImportStoreReceiptData(importAsrsRequest).ConfigureAwait(false);
            if (!result.Success)
            {
                _logger.LogError("Failed to import StoreReceipt excel data");
                return BadRequest();
            }
            _logger.LogInformation("File imported successfully");
            return Ok(result.Message);

        }

        [HttpPost]
        [Authorize]
        [Route("AddStoreReceiptData")]
        public async Task<IActionResult> AddStoreReceiptRequestData([FromForm]StoreReceiptRequest storeReceiptRequest)
        {
            _logger.LogInformation("Inserting StoreReceipt data");
            var result = await _storeReceiptService.AddStoreReceiptData(storeReceiptRequest).ConfigureAwait(false);
            if (!result.Success)
            {
                _logger.LogError("Failed to insert StoreReceipt data");
                return BadRequest();
            }
            _logger.LogInformation("Store receipt data inserted successfully");
            return Ok(result.Message);

        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAllStoreReceipts()
        {
            var result = await _storeReceiptService.GetAllStoreReceipts();
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByStoreReceiptNo/{storeReceiptNo:int}")]
        public async Task<IActionResult> GetStoreReceiptByStoreReceiptNo([Required] int storeReceiptNo)
        {
            var result = await _storeReceiptService.GetStoreReceiptByStoreReceiptNo(storeReceiptNo).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByLocationNo/{locationNo}")]
        public async Task<IActionResult> GetStoreReceiptByLocation([Required]string locationNo)
        {
            var result = await _storeReceiptService.GetStoreReceiptByLocationNo(locationNo).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByStatus/{transcationStatus}")]
        public async Task<IActionResult> GetStoreReceiptByStatus([Required] string transcationStatus)
        {
            var result = await _storeReceiptService.GetStoreReceiptByTranscationStatus(transcationStatus).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByMonthAndYear/{month:int}/{year:int}")]
        public async Task<IActionResult> GetStoreReceiptByMonthAndYear([Required] int month, int year)
        {
            var result = await _storeReceiptService.GetStoreReceiptByMonthAndYearAsync(month, year).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpPut("UpdateStoreReceipt/{storeReceiptNo: int}")]
        public async Task<IActionResult> UpdateStoreReceipt([Required]int storeReceiptNo, [FromBody]StoreReceiptRequest request)
        {
            var result = await _storeReceiptService.UpdateStoreReceipt(request).ConfigureAwait(false);
            if (!result.Success) 
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

    }
}
