using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class TiposEstado
    {
        public TiposEstado()
        {
            Estados = new HashSet<Estado>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Estado> Estados { get; set; }
    }
}
