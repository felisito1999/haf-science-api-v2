using haf_science_api.Interfaces;
using haf_science_api.Models;
using haf_science_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly IDataService<Estado, EstadosView> _estadosService;
        public EstadosController(IDataService<Estado, EstadosView> estadosService)
        {
            _estadosService = estadosService;
        }
        [Route("get")]
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadosView>>> Get()
        {
            var estados = await _estadosService.GetAll();

            return Ok(estados);
        }
        //public async Task<ActionResult<string>> Get()
        //{
        //    //var estados = await _estadosService.GetData();

        //    return Ok("This is a test");
        //}
    }
}
