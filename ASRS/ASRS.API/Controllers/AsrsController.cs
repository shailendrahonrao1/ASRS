using ASRS.Application.Interfaces;
using ASRS.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsrsController : ControllerBase
    {
        private readonly IAsrsService asrsService;

        public AsrsController(IAsrsService asrsService)
        {
            this.asrsService = asrsService;
        }
        [HttpPost("importStoreReceiptRequests")]
        public async Task<IActionResult> ImportStoreReceiptRequestData(ImportAsrsRequest importAsrsRequest) 
        {
            var (result, message) = await asrsService.ImportStoreReceiptData(importAsrsRequest);
            if(!result)
            {
                return BadRequest(message);
            }
            return Ok(message);
        }
    }
}
