using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class ActividadUsuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int TipoActividadId { get; set; }
        public bool Eliminado { get; set; }

        public virtual TipoActividad TipoActividad { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
