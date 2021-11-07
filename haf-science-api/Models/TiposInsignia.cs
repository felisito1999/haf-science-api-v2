using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class TiposInsignia
    {
        public TiposInsignia()
        {
            Insignia = new HashSet<Insignia>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Insignia> Insignia { get; set; }
    }
}
