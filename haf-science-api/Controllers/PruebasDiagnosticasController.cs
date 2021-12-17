using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        private readonly IUserService<UsuariosModel> _usersService;

        public PruebasDiagnosticasController(IPruebasDiagnosticasService<PruebasDiagnostica> pruebasDiagnosticasService,
            IUserService<UsuariosModel> usersService)
        {
            _pruebasDiagnosticasService = pruebasDiagnosticasService;
            _usersService = usersService;
        }
        [Route("test-details")]
        [HttpGet]
        [Authorize(Roles = "Docente,Estudiante")]
        public async Task<IActionResult> GetSessionTestDetail(int testId, int sessionId)
        {
            try
            {
                var pruebaDiagnostica = await _pruebasDiagnosticasService.GetSessionPruebaDiagnosticaById(testId, sessionId);

                return Ok(pruebaDiagnostica);
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
        [Authorize(Roles = "Docente,Estudiante")]
        public async Task<ActionResult> GetSessionPruebasDiagnosticas(int page, int pageSize, int sessionId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var roleValue = claimsIdentity.Claims.ToArray()[8].Value;
                int userId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                if (roleValue == "Estudiante")
                {
                    var pruebasDiagnosticas = await _pruebasDiagnosticasService
                    .GetPaginatedStudentPruebasDiagnosticasBySessionId(userId, sessionId, page, pageSize);
                    var pruebasDiagnosticasCount = await _pruebasDiagnosticasService
                        .GetPaginatedStudentPruebasDiagnosticasBySessionIdCount(sessionId);

                    return Ok(new PaginatedResponse<object>()
                    {
                        Records = pruebasDiagnosticas,
                        RecordsTotal = pruebasDiagnosticasCount
                    });
                }
                else
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
        [Route("get-attempt")]
        [HttpGet]
        [Authorize(Roles = "Estudiante")]
        public async Task<ActionResult> GetAttempt(int pruebaDiagnosticaId, int sessionId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int studentId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var test = await _pruebasDiagnosticasService.IsAvailableForStudent(studentId, sessionId, pruebaDiagnosticaId);
                
                if (test != null)
                {
                    var student = await _usersService.GetUsuarioById(studentId); 
                    await _pruebasDiagnosticasService.StartTest(pruebaDiagnosticaId, student, sessionId);
                    return Ok(test);
                }
                throw new Exception("Esta prueba no esta disponible");
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
        [Route("submit-attempt")]
        [HttpPost]
        [Authorize(Roles = "Estudiante")]
        public async Task<ActionResult> SubmitAttempt(AttemptModel attemptInfo)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int studentId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                await _pruebasDiagnosticasService.EvaluateTestGrade(attemptInfo, attemptInfo.SessionId, studentId);

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
        [Route("session-average-grades")]
        [HttpGet]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> GetSessionAverageGrades(int sessionId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var grades = await _pruebasDiagnosticasService.GetSessionAverageGrades(sessionId, teacherId);

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response() 
                    { 
                        Status= "Error",
                        Message= ex.Message
                    });
                throw;
            }
        }
        [Route("session-test-average-grades")]
        [HttpGet]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> GetSessionTestAverageGrades(int sessionId, int testId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var grades = await _pruebasDiagnosticasService.GetSessionTestAverageGrades(sessionId, testId, teacherId);

                return Ok(grades);
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
        [Route("student-session-average-grades")]
        [HttpGet]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> GetStudentSessionAverageGrades(int sessionId, int studentId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var grades = await _pruebasDiagnosticasService.GetStudentSessionAverageGrades(sessionId, studentId, teacherId);

                return Ok(grades);
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
        [Route("session-test-grades")]
        [HttpGet]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> GetTestSessionGrades(int sessionId, int testId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var grades = await _pruebasDiagnosticasService.GetTestSessionGrades(testId, sessionId, teacherId);

                return Ok(grades);
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
    }
}
