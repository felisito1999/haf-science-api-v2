using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Insignia = new HashSet<Insignia>();
            InverseCreadoPorNavigation = new HashSet<Usuario>();
            Pregunta = new HashSet<Pregunta>();
            PruebasDiagnosticas = new HashSet<PruebasDiagnostica>();
            Respuesta = new HashSet<Respuesta>();
            Sesiones = new HashSet<Sesione>();
            UserHashes = new HashSet<UserHash>();
            UsuariosInsigniaAsignadoPorNavigations = new HashSet<UsuariosInsignia>();
            UsuariosInsigniaUsuarios = new HashSet<UsuariosInsignia>();
            UsuariosLogs = new HashSet<UsuariosLog>();
            UsuariosSesiones = new HashSet<UsuariosSesione>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string NombreUsuario { get; set; }
        public string Salt { get; set; }
        public string Contrasena { get; set; }
        public int RolId { get; set; }
        public int UsuarioDetalleId { get; set; }
        public int EstadoId { get; set; }
        public bool EsSuperAdministrador { get; set; }
        public int? CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Role Rol { get; set; }
        public virtual UsuariosDetalle UsuarioDetalle { get; set; }
        public virtual ICollection<Insignia> Insignia { get; set; }
        public virtual ICollection<Usuario> InverseCreadoPorNavigation { get; set; }
        public virtual ICollection<Pregunta> Pregunta { get; set; }
        public virtual ICollection<PruebasDiagnostica> PruebasDiagnosticas { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }
        public virtual ICollection<Sesione> Sesiones { get; set; }
        public virtual ICollection<UserHash> UserHashes { get; set; }
        public virtual ICollection<UsuariosInsignia> UsuariosInsigniaAsignadoPorNavigations { get; set; }
        public virtual ICollection<UsuariosInsignia> UsuariosInsigniaUsuarios { get; set; }
        public virtual ICollection<UsuariosLog> UsuariosLogs { get; set; }
        public virtual ICollection<UsuariosSesione> UsuariosSesiones { get; set; }
    }
}
