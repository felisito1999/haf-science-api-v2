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
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService<UsuarioModel> _usersService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AuthenticationController(IUserService<UsuarioModel> usersService, IMapper mapper, ITokenService tokenService)
        {
            _usersService = usersService;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("register")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Register([FromBody] RegistrationModel model)
        {
            var userExists = await _usersService.GetUsuarioByUsername(model.NombreUsuario);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "No se pudo registrar el usuario" });
            }

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            model.CreadoPor = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

            await _usersService.Register(_mapper.Map<UsuarioModel>(model));

            return StatusCode(StatusCodes.Status200OK,
                new Response { Status = "Success", Message = "Se ha registrado el usuario exitosamente" });
        }
        [HttpPost]
        [Route("token")]
        public async Task<ActionResult> GetToken([FromBody] LoginData requestUser)
        {
            if (requestUser != null && requestUser.NombreUsuario != null && requestUser.Contrasena != null)
            {
                var user = await _usersService.GetUsuarioLoginInfo(requestUser.NombreUsuario, requestUser.Contrasena);

                if (user != null)
                {
                    var token = _tokenService.WriteToken(user);
                    return Ok(
                        new TokenResponse
                        {
                            Status = "Success",
                            Token = token,
                            UserInfo = _mapper.Map<UserInfo>(user)
                        });
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized,
                        new Response { Status = "Unauthorized", Message = "No se pudo iniciar sesión, revise el usuario y la contraseña ingresados." });
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "Credenciales inválidas" });
            }
        }
        [HttpPost]
        [Route("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            return Ok();
        }
        [Route("info")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> UserInfo()
        {
            int loggedUserId = Convert.ToInt32((this.User.Identity as ClaimsIdentity).FindFirst("id").Value);
            var user = _mapper.Map<UserInfo>(await _usersService.GetUsuarioById(loggedUserId));

            return Ok(user);
        }
        [HttpPost]
        [Route("check")]
        public async Task<ActionResult> CheckUserIsLoggedIn()
        {
            var isUserLoggedIn = this.User.Identity.IsAuthenticated;

            return Ok(isUserLoggedIn);
        }
    }
}
