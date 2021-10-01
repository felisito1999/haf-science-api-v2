using haf_science_api.Interfaces;
using haf_science_api.Models;
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
    public class CentrosEducativosController : ControllerBase
    {
        private readonly IDataService<CentrosEducativo> _centrosEducativosService;

        public CentrosEducativosController(IDataService<CentrosEducativo> centrosEducativosService)
        {
            _centrosEducativosService = centrosEducativosService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get(int? id, string name)
        {
            try
            {
                if (id.HasValue)
                {
                    return Ok( await _centrosEducativosService.GetById(id));
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    return Ok(await _centrosEducativosService.GetByName(name));
                }
                else
                {
                    return Ok(await _centrosEducativosService.GetAll());
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
