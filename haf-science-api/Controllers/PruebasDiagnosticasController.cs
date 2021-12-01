using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PruebasDiagnosticasController : ControllerBase
    {
        private readonly IPruebasDiagnosticasService<PruebasDiagnostica> _pruebasDiagnosticasService;   
        
        public PruebasDiagnosticasController(IPruebasDiagnosticasService<PruebasDiagnostica> pruebasDiagnosticasService)
        {
            _pruebasDiagnosticasService = pruebasDiagnosticasService;
        }

        [Route("teacher-pruebas-diagnosticas")]
        [HttpGet]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> GetTeacherPruebasDiagnosticas(int page, int pageSize)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var pruebasDiagnosticas = await _pruebasDiagnosticasService
                    .GetTeacherPaginatedPruebasDiagnosticas(teacherId, page, pageSize);
                var pruebasDiagnosticasCount = await _pruebasDiagnosticasService
                    .GetTeacherPaginatedPruebasDiagnosticasCount(teacherId);

                return Ok(new PaginatedResponse<PruebasDiagnostica>()
                {
                    Records = pruebasDiagnosticas,
                    RecordsTotal = pruebasDiagnosticasCount
                });
            }
            catch (Exception ex)
            {

                throw;
            }        
        }
        [Route("session-pruebas-diagnosticas")]
        [HttpGet]
        public async Task<ActionResult> GetSessionPruebasDiagnosticas(int page, int pageSize, int sessionId)
        {
            try
            {
                var pruebasDiagnosticas = await _pruebasDiagnosticasService
                    .GetPaginatedPruebasDiagnosticasBySessionId(sessionId, page, pageSize);
                var pruebasDiagnosticasCount = await _pruebasDiagnosticasService
                    .GetPaginatedPruebasDiagnosticasBySessionIdCount(sessionId);

                return Ok(new PaginatedResponse<object>()
                {
                    Records = pruebasDiagnosticas,
                    RecordsTotal = pruebasDiagnosticasCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.Message
                    });
                throw;
            }
        }
        [Route("save-prueba-diagnostica")]
        [HttpPost]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> SavePruebaDiagnostica(PruebasDiagnosticasModel pruebaDiagnostica)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                pruebaDiagnostica.PruebaDiagnostica.CreadoPor = teacherId;
                pruebaDiagnostica.PruebaDiagnostica.EstadoId = 11;
                await _pruebasDiagnosticasService.SavePruebaDiagnostica(pruebaDiagnostica, teacherId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.Message.ToString()
                    });
                throw;
            }
        
        }
        [Route("assign-to-session")]
        [HttpPost]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> AssignPruebaToSession(AssignTestToSessionModel assignTestToSessionModel)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                await _pruebasDiagnosticasService.AssignToSession(assignTestToSessionModel, teacherId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response()
                    {
                        Status = "Error",
                        Message = ex.Message.ToString()
                    });
                throw;
            }
        }
    }
}
