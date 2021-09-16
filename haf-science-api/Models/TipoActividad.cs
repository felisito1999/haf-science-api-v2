using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class TipoActividad
    {
        public TipoActividad()
        {
            ActividadUsuarios = new HashSet<ActividadUsuario>();
        }

        public int Id { get; set; }
        public int NombreTipoActividad { get; set; }
        public string Descripcion { get; set; }
        public bool Elimiando { get; set; }

        public virtual ICollection<ActividadUsuario> ActividadUsuarios { get; set; }
    }
}
