using Microsoft.AspNetCore.Mvc;
using Proje1.DBContext;
using Proje1.FormModel;
using Proje1.Services;

namespace Proje1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private CustomerService service;
        public CustomerController(SalesDBContext _context, IConfiguration config)
        {
            service = new CustomerService(_context, config);
        }

        [HttpGet("Customer")]
        public IActionResult getCustomer()
        {
            var data = service.getCustomer();
            return Ok(data);
        }

        [HttpPost("Insert-customer")]
        public async Task<IActionResult> InsertCustomer([FromForm] CustomerFormModel model)
        {
            var data = await service.InsertCustomer(model);
            return Ok(data);
        }

        [HttpPost("multiple-customer")]
        public async Task<IActionResult> InsertMultipleCustomerAsync([FromForm] MultipleCustomer model)
        {

            var data = await service.InsertMultipleCustomerAsync(model);

            return Ok(data);

        }

        [HttpDelete("delete-customer")]
        public async Task<IActionResult> DeleteCustomerAsync([FromForm] CustomerFormModel model)
        {
            var data = await service.DeleteCustomerAsync(model);
            return Ok(data);
        }
    }
}
