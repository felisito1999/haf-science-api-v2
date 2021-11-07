using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class UsuariosSesione
    {
        public int UsuarioId { get; set; }
        public int SesionId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreSesion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Sesione Sesion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
