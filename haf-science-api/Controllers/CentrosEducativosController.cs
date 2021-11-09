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
    public class CentrosEducativosController : ControllerBase
    {
        private readonly ICentrosEducativosService<CentrosEducativosModel, PaginatedCentrosEducativosView> _centrosEducativosService;
        private readonly ILogger _logger;

        public CentrosEducativosController(ICentrosEducativosService<CentrosEducativosModel, PaginatedCentrosEducativosView> centrosEducativosService,
        ILogger<CentrosEducativosController> logger)
        {
            _centrosEducativosService = centrosEducativosService;
            _logger = logger; 
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var centrosEducativos = await _centrosEducativosService.GetAll();

                return Ok(centrosEducativos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError,
                new Response()
                {
                    Status = "Error",
                    Message = "Ha ocurrido un error"
                }); ;
                throw;
            }
        }
        [HttpGet]
        [Route("GetByName")]
        [Authorize(Roles = "Administrador,Docente")]
        public async Task<ActionResult> GetByName(string name)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var centrosEducativos = await _centrosEducativosService.GetByName(name);

                    return Ok(centrosEducativos);
                }
                else
                {
                    return BadRequest(new Response()
                    {
                        Status = "Error",
                        Message = "No se envío el parámetro de nombre"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response() { 
                        Status = "Error",
                        Message = ex.ToString()
                    });

                throw;
            }
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Get(int page, int pageSize, int? id, string name)
        {
            try
            {
                if (id.HasValue)
                {
                    return Ok( await _centrosEducativosService.GetById(id));
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    var schools = await _centrosEducativosService.GetPaginatedDataBy(page, pageSize, name);
                    var totalRecords = await _centrosEducativosService.GetPaginatedUsersCountBy(name);

                    return StatusCode(StatusCodes.Status200OK,
                    new PaginatedResponse<PaginatedCentrosEducativosView> { Records = schools, RecordsTotal = totalRecords });
                }
                else
                {
                    var schools = await _centrosEducativosService.GetPaginatedData(page, pageSize);
                    var totalRecords = await _centrosEducativosService.GetPaginatedUsersCount();

                    return StatusCode(StatusCodes.Status200OK,
                        new PaginatedResponse<PaginatedCentrosEducativosView> { Records = schools, RecordsTotal = totalRecords });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.ToString()
                    });

                throw;
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> SaveSchool([FromBody] CentrosEducativosModel centroEducativo)
        {
            try
            {
                await _centrosEducativosService.SaveSingle(centroEducativo);

                return Ok(new Response()
                {
                    Status = "Success",
                    Message = "El centro educativo ha sido agregado satisfactoriamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.ToString()
                    });
                throw;
            }
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> UpdateSchool([FromBody] CentrosEducativosModel centrosEducativo)
        {
            try
            {
                await _centrosEducativosService.Update(centrosEducativo);

                return Ok(new Response()
                {
                    Status = "Success",
                    Message = "El centro educativo fue actualizado con satisfactoriamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.ToString()
                    });
                throw;
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteSchool(int id)
        {
            try
            {
                await _centrosEducativosService.Delete(id);

                return Ok(new Response()
                {
                    Status = "Success",
                    Message = "El centro educativo fue deshabilitado satisfactoriamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.ToString()
                    });
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetTiposCentros")]
        public async Task<ActionResult> GetTiposCentrosEducativos()
        {
            try
            {
                var tiposCentrosEducativos = await _centrosEducativosService
                    .GetAllTiposCentrosEducativos();

                return Ok(tiposCentrosEducativos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.ToString()
                    });
                throw;
            }
        }
        [HttpGet]
        [Authorize]
        [Route("GetRegionales")]
        public async Task<ActionResult> GetRegionales()
        {
            try
            {
                var regionales = await _centrosEducativosService.GetAllRegionales();

                return Ok(regionales);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.ToString()
                    });
                throw;
            }
        }
        [HttpGet]
        [Authorize]
        [Route("GetDistritos")]
        public async Task<ActionResult> GetDistritos()
        {
            try
            {
                var regionales = await _centrosEducativosService.GetAllDistritos();

                return Ok(regionales);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.ToString()
                    });
                throw;
            }
        }
    }
}
