using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Role
    {
        public Role()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int EstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
