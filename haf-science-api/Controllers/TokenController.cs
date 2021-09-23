using haf_science_api.Models;
using haf_science_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using haf_science_api.Viewmodels;
using haf_science_api.Interfaces;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUserService<UsuarioModel> _usersService; 
 
        public TokenController(IConfiguration configuration, IUserService<UsuarioModel> usersService)
        {
            _configuration = configuration;
            _usersService = usersService;
        }
        [HttpPost]  
        public async Task<IActionResult> Post([FromBody]UserInfo _user)
        {
            if(_user != null && _user.NombreUsuario != null && _user.Contrasena != null)
            {
                var user = await GetUserLoginInfo(_user.NombreUsuario, _user.Contrasena);

                if (user != null)                
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("id", user.Id.ToString()),
                        new Claim("firstName", user.Nombres),
                        new Claim("lastName", user.Apellidos),
                        new Claim("userName", user.NombreUsuario),
                        new Claim("email", user.CorreoElectronico),
                        new Claim("role", user.Rol)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(7), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Credenciales invalidas");
                }
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<UsuarioModel> GetUserLoginInfo(string username, string password)
        {
            var user = await _usersService.GetUsuarioLoginInfo(username, password);

            return user;
        }
    }
}
