using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class UsuariosLog
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TipoLog { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual TiposLog TipoLogNavigation { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
