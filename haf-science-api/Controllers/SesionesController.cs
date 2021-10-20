using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionesController : ControllerBase
    {
        private readonly ISessionService<SesionesModel, PaginatedSesionesView> _sessionService;
        private readonly ILogger _logger;

        public SesionesController(ISessionService<SesionesModel, PaginatedSesionesView> sessionService, 
            ILogger<SesionesController> logger)
        {
            _sessionService = sessionService;
            _logger = logger; 
        }
        [Authorize]
        public async Task<ActionResult> Get([FromQuery] int page, [FromQuery] int pageSize, int? id, int? centroEducativoId, string name)
        {
            try
            {
                
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var roleValue = claimsIdentity.Claims.ToArray()[8].Value;

                if (roleValue == "Administrador")
                {
                    if (id.HasValue)
                    {
                        var session = await _sessionService.GetById((int)id);

                        if (session == null)
                        {
                            return NotFound();
                        }

                        return Ok(session);
                    }
                    if (!string.IsNullOrWhiteSpace(name) | centroEducativoId.HasValue)
                    {
                        var sessions = await _sessionService.GetPaginatedSessionsBy(page, pageSize, name, centroEducativoId);
                        var totalRecords = await _sessionService.GetPaginatedSessionsCountBy(name, centroEducativoId);

                        return StatusCode(StatusCodes.Status200OK,
                            new PaginatedResponse<PaginatedSesionesView> { Records = sessions, RecordsTotal = totalRecords });
                    }
                    else
                    {
                        var sessions = await _sessionService.GetPaginatedSessions(page, pageSize);
                        var totalRecords = await _sessionService.GetPaginatedSessionsCount();

                        return StatusCode(StatusCodes.Status200OK,
                            new PaginatedResponse<PaginatedSesionesView> { Records = sessions, RecordsTotal = totalRecords });
                    }
                }
                else if (roleValue == "Docente")
                {
                    var teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                    var sessions = await _sessionService.GetPaginatedTeacherSessions(page, pageSize, teacherId);
                    var totalRecords = await _sessionService.GetPaginatedTeacherSessionsCount(teacherId);

                    return StatusCode(StatusCodes.Status200OK,
                        new PaginatedResponse<PaginatedSesionesView> { Records = sessions, RecordsTotal = totalRecords });
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Update([FromBody] SesionesModel session)
        {
            if (session != null)
            {
                await _sessionService.Update(session);

                return Ok();
            }

            return BadRequest();
        }
        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _sessionService.Delete(id);

                return Ok(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                throw;
            }
        }

    }
}
