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

        public SesionesController(ISessionService<SesionesModel, PaginatedSesionesView> sessionService, 
            ILogger<SesionesController> logger)
        {
            _sessionService = sessionService;
        }
        [Authorize]
        public async Task<ActionResult> Get(int page, int pageSize, int? id, int? centroEducativoId, string name)
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

                    if (id.HasValue)
                    {
                        var session = await _sessionService.GetById((int)id);

                        return Ok(session);
                    }
                    else if (!string.IsNullOrWhiteSpace(name))
                    {
                        var sessions = await _sessionService.GetPaginatedTeacherSessionsDataBy(page, pageSize, teacherId, name);
                        var totalRecords = await _sessionService.GetPaginatedTeacherSessionsCountBy(teacherId, name);

                        return StatusCode(StatusCodes.Status200OK,
                            new PaginatedResponse<PaginatedSesionesView> { Records = sessions, RecordsTotal = totalRecords });
                    }
                    else
                    {
                        var sessions = await _sessionService.GetPaginatedTeacherSessions(page, pageSize, teacherId);
                        var totalRecords = await _sessionService.GetPaginatedTeacherSessionsCount(teacherId);

                        return StatusCode(StatusCodes.Status200OK,
                            new PaginatedResponse<PaginatedSesionesView> { Records = sessions, RecordsTotal = totalRecords });
                    }
                }
                else 
                {
                    var studentId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                    var sessions = await _sessionService.GetPaginatedStudentSessionsDataBy(page, pageSize, studentId);
                    var totalRecords = _sessionService.GetPaginatedStudentSessionsCountBy(studentId);
                    return Ok();
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
        public async Task<ActionResult> Save(SessionSaveModel session)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);
                await _sessionService.Save(session, teacherId);
                return Ok();
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
        public async Task<ActionResult> Update([FromBody] SesionesModel session)
        {
            try
            {
                if (session != null)
                {
                    await _sessionService.Update(session);

                    return Ok();
                }

                return BadRequest();
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
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _sessionService.Delete(id);

                return Ok();
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
