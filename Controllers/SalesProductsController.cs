using Microsoft.AspNetCore.Mvc;
using Proje1.DBContext;
using Proje1.FormModel;
using Proje1.Services;

namespace Proje1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesProductsController : ControllerBase
    {
        private SalesProductsService service;
        public SalesProductsController(SalesDBContext _context, IConfiguration config)
        {
            service = new SalesProductsService(_context, config);
        }

        [HttpGet("SalesProducts")]
        public IActionResult getSalesProducts()
        {
            var data = service.getSalesProducts();
            return Ok(data);
        }

        [HttpPost("Insert-products")]
        public async Task<IActionResult> InsertProductAsync([FromForm] SalesProductFormModel model)
        {
            var data = await service.InsertProductAsync(model);
            return Ok(data);
        }

        [HttpPost("multiple-product")]
        public async Task<IActionResult> InsertMultipleProductAsync([FromForm] MultipleProduct model)
        {
            var data = await service.InsertMultipleProductAsync(model);

            return Ok(data);
        }

        [HttpDelete("delete-products")]
        public async Task<IActionResult> DeleteProductAsync([FromForm] SalesProductFormModel model)
        {
            var data = await service.DeleteProductAsync(model);
            return Ok(data);
        }


    }
}
