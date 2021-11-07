using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class UsuariosInsignia
    {
        public int UsuarioId { get; set; }
        public int InsigniaId { get; set; }
        public int AsignadoPor { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreInsignia { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Usuario AsignadoPorNavigation { get; set; }
        public virtual Insignia Insignia { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
