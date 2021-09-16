using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class UsuariosDetalle
    {
        public UsuariosDetalle()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
