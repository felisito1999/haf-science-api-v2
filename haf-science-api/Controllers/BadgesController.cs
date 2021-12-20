using System; 
using haf_science_api.Interfaces;
using haf_science_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using haf_science_api.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BadgesController : ControllerBase
    {
        private readonly IInsigniasService<Insignia> _insigniasService;

        public BadgesController(IInsigniasService<Insignia> insigniasService)
        {
            _insigniasService = insigniasService;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok();
        }
        [HttpGet]
        [Route("imagenes-insignias")]
        public async Task<ActionResult> GetImagenesInsignias()
        {
            try
            {
                var imagenesInsignias = await _insigniasService.GetImagenesInsignias();

                return Ok(imagenesInsignias);
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
        [Route("get-my-created-badges")]
        public async Task<ActionResult> GetMyCreatedInsignias()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var sessionInsignias = await _insigniasService.GetInsigniaCreatedBy(teacherId);

                return Ok(sessionInsignias);
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
        [Route("save-badge")]
        public async Task<ActionResult> SaveInsignia(Insignia insignia)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                insignia.CreadoPor = teacherId;
                insignia.Eliminado = false; 
                insignia.FechaCreacion = DateTime.Now;
                insignia.TipoInsigniaId = 1;
                insignia.EstadoId = 17;
                await _insigniasService.SaveBadge(insignia);

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
        [HttpPost]
        [Route("assign-badge")]
        public async Task<ActionResult> AssignBadge(UsuariosSesionesInsignia usuariosSesionesInsignia)
        {
            try
            {
                await _insigniasService.AssignInsigniaToSessionStudent(usuariosSesionesInsignia);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response() { 
                        Status = "Error", 
                        Message = ex.Message 
                    });
                throw;
            }
        }
        [HttpGet]
        [Route("getFavoriteBadge")]
        public async Task<ActionResult> GetFavoriteBadge(int studentId, int sessionId)
        {
            try
            {
                var insignia = await _insigniasService.GetFavoriteSessionUserInsignia(studentId, sessionId);

                return Ok(insignia);
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
        [Route("get-student-badges")]
        [Authorize(Roles = "Estudiante")]
        public async Task<ActionResult> GetStudentBadges(int sessionId)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int studentId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                var insignias = await _insigniasService.GetStudentSessionsInsignias(studentId, sessionId);

                return Ok(insignias);
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
