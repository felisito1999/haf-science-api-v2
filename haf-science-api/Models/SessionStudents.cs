using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    [Keyless]
    public class SessionStudents
    {
        public int UsuarioId { get; set; }
        public int SessionId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreSesion { get; set; }
    }
}
