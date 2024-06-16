using ASRS.Application.Interfaces;
using ASRS.Application.Services;
using ASRS.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockReleaseController : ControllerBase
    {
        private readonly IStockReleaseService _stockReleaseService;
        private readonly ILogger<StockReleaseController> _logger;

        public StockReleaseController(IStockReleaseService stockReleaseService, ILogger<StockReleaseController> logger)
        {
            _stockReleaseService = stockReleaseService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [Route("ImportStockReleaseData")]
        public async Task<IActionResult> ImportStockReleaseRequestData(ImportAsrsRequest importAsrsRequest)
        {
            _logger.LogInformation("Importing StockRelease excel data");
            var result = await _stockReleaseService.ImportStockReleaseData(importAsrsRequest).ConfigureAwait(false);
            if (!result.Success)
            {
                _logger.LogError("Failed to import StockRelease excel data");
                return BadRequest();
            }
            _logger.LogInformation("File imported successfully");
            return Ok(result.Message);
        }

        [HttpPost]
        [Authorize]
        [Route("AddStockReleaseData")]
        public async Task<IActionResult> AddStockReleaseRequestData([FromForm]StockReleaseRequest stockReleaseRequest)
        {
            _logger.LogInformation("Inserting StockRelease data");
            var result = await _stockReleaseService.AddStockReleaseData(stockReleaseRequest).ConfigureAwait(false);
            if (!result.Success)
            {
                _logger.LogError("Failed to insert StockRelease data");
                return BadRequest();
            }
            _logger.LogInformation("Stock release data inserted successfully");
            return Ok(result.Message);
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetAllStockRelease()
        {
            var result = await _stockReleaseService.GetAllStockRelease();
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByStockReleaseNo/{stockReleaseNo:int}")]
        public async Task<IActionResult> GetStockReleaseByStockReleaseNo([Required] int stockReleaseNo)
        {
            var result = await _stockReleaseService.GetStockReleaseByStockReleaseNo(stockReleaseNo).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByLocationNo/{locationNo}")]
        public async Task<IActionResult> GetStockReleaseByLocation([Required] string locationNo)
        {
            var result = await _stockReleaseService.GetStockReleaseByLocationNo(locationNo).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByDemandNo/{demandNo}")]
        public async Task<IActionResult> GetStockReleaseByDemandNo([Required] string demandNo)
        {
            var result = await _stockReleaseService.GetStockReleaseByDemandNo(demandNo).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByIssueVoucherNo/{issueVoucherNo}")]
        public async Task<IActionResult> GetStockReleaseByIssueVoucherNo([Required] string issueVoucherNo)
        {
            var result = await _stockReleaseService.GetStockReleaseByIssueVoucherNo(issueVoucherNo).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByStatus/{transcationStatus}")]
        public async Task<IActionResult> GetStockReleaseByStatus([Required] string transcationStatus)
        {
            var result = await _stockReleaseService.GetStockReleaseByTranscationStatus(transcationStatus).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpGet("GetByMonthAndYear/{month:int}/{year:int}")]
        public async Task<IActionResult> GetStockReleaseByMonthAndYear([Required] int month, int year)
        {
            var result = await _stockReleaseService.GetStockReleaseByMonthAndYear(month, year).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStockRelease(StockReleaseRequest request)
        {
            var result = await _stockReleaseService.UpdateStockRelease(request).ConfigureAwait(false);
            if (!result.Success)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }
    }
}
