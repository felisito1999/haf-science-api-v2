using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Estado
    {
        public Estado()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TipoEstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual TiposEstado TipoEstado { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
