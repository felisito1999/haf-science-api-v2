using AutoMapper;
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
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService<UsuarioModel> _usuariosService;
        private readonly IMapper _mapper;
        public AuthenticationController(IUserService<UsuarioModel> usuariosService, IMapper mapper)
        {
            _usuariosService = usuariosService;
            _mapper = mapper;
        }
        //[Route("api/auth/register")]
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            var userExists = await _usuariosService.GetUsuarioByUsername(model.NombreUsuario);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "No se pudo registrar el usuario" });
            }

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            model.CreadoPor = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

            await _usuariosService.Register(_mapper.Map<UsuarioModel>(model));

            return Ok();
        }
    }
}
