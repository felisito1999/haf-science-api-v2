using haf_science_api.Interfaces;
using haf_science_api.Models;
using haf_science_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProvinciasController : ControllerBase
    {
        private readonly IProvinciasService<Provincia> _provinciasService;
        private readonly ILogger _logger; 
        public ProvinciasController(IProvinciasService<Provincia> provinciasService, ILogger<ProvinciasController> logger)
        {
            _provinciasService = provinciasService;
            _logger = logger; 
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var provincias = await _provinciasService.GetAll();
                return Ok(provincias);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
