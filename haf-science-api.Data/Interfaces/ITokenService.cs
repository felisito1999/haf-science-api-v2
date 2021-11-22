using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface ITokenService
    {
        public string WriteToken(UsuariosModel user);
    }
}
