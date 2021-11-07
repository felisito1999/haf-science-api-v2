using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class TiposCentrosEducativo
    {
        public TiposCentrosEducativo()
        {
            CentrosEducativos = new HashSet<CentrosEducativo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<CentrosEducativo> CentrosEducativos { get; set; }
    }
}
