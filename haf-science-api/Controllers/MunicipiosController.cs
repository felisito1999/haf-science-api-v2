using haf_science_api.Interfaces;
using haf_science_api.Models;
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
    public class MunicipiosController : ControllerBase
    {
        private readonly IMunicipiosService<Municipio> _municipiosService;
        private readonly ILogger _logger;

        public MunicipiosController(IMunicipiosService<Municipio> municipiosService, ILogger<MunicipiosController> logger)
        {
            _municipiosService = municipiosService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _municipiosService.GetAll());
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "Ha ocurrido un error"
                    });
                throw;
            }
        }
        [HttpGet]
        [Route("GetByProvinciaId")]
        public async Task<ActionResult> GetMunicipiosByProvinciaId(int provinciaId)
        {
            try
            {
                return Ok(await _municipiosService.GetMuncipiosByProvinciaId(provinciaId)); 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = "Ha ocurrido un error"
                    });
                throw;
            }
        }
    }
}
