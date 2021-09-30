using haf_science_api.Interfaces;
using haf_science_api.Models;
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
    public class CentrosEducativosController : ControllerBase
    {
        private readonly IDataService<CentrosEducativo> _centrosEducativosService;

        public CentrosEducativosController(IDataService<CentrosEducativo> centrosEducativosService)
        {
            _centrosEducativosService = centrosEducativosService; 
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var centrosEducativos = await _centrosEducativosService.GetAll();

                return Ok(centrosEducativos);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var centroEducativo = await _centrosEducativosService.GetById(id);

                return Ok(centroEducativo);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
    }
}
