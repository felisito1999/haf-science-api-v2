using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            ActividadUsuarios = new HashSet<ActividadUsuario>();
        }

        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Salt { get; set; }
        public string Contrasena { get; set; }
        public int CentroEducativoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int RolId { get; set; }
        public int UsuarioDetalleId { get; set; }
        public int EstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual CentrosEducativo CentroEducativo { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Role Rol { get; set; }
        public virtual UsuariosDetalle UsuarioDetalle { get; set; }
        public virtual ICollection<ActividadUsuario> ActividadUsuarios { get; set; }
    }
}
