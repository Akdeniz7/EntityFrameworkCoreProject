using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje1.DBContext;
using Proje1.FormModel;
using Proje1.Models;
using Proje1.Services;
using System;



namespace Proje1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalerController : ControllerBase
    {
        private SalerService service;
        private readonly SalerDto profile;
        private readonly SalerService authService;
        public SalerController(SalesDBContext _context, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            string token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if(!string.IsNullOrEmpty(token) )
            {
                token = token.Replace("Bearer ", "");
            }

            authService = new SalerService(_context, config, profile);

            profile = authService.ResolveUserToken(token, "SalerController");

            service = new SalerService(_context, config, profile);
        }

        [HttpPost("login")]
        public IActionResult Login([FromForm] Login model)
        {
            var data = service.Login(model);
            return Ok(data);
        }

        [HttpGet("Saler")]
        public IActionResult getSaler()
        {
            var data = service.getSaler();
            return Ok(data);
        }
        [HttpGet("saler-profile")]
        [Authorize]
        public IActionResult getSalerProfile()
        {
            var data = service.getSalerProfile();
            return Ok(data);
        }


        [HttpPost("Insert-saler")]
        public async Task<IActionResult> InsertSalerAsync([FromForm] SalerFormModel model)
        {
            var data = await service.InsertSalerAsync(model);
            return Ok(data);
        }

        [HttpPost("multiple-saler")]
        public async Task<IActionResult> InsertMultipleSalerAsync([FromForm] MultipleSaler model)
        {
            var data = await service.InsertMultipleSalerAsync(model);
            return Ok(data);
        }

        [HttpDelete("delete-saler")]
        public async Task<IActionResult> DeleteSalerAsync([FromForm] SalerFormModel model)
        {
            var data = await service.DeleteSalerAsync(model);
            return Ok(data);
        }

        [HttpDelete("delete-multiple-saler")]
        public async Task<IActionResult> DeleteMultipleSalerAsync([FromForm] MultipleSaler model)
        {
            var data = await service.DeleteMultipleSalerAsync(model);
            return Ok(data);
        }
    }
}