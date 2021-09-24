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
using haf_science_api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUserService<UsuarioModel> _usersService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
 
        public TokenController(IConfiguration configuration, IUserService<UsuarioModel> usersService, ITokenService tokenService,
            IMapper mapper)
        {
            _configuration = configuration;
            _usersService = usersService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [Route("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            return Ok();
        }
    }
}
