using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Provincia
    {
        public Provincia()
        {
            CentrosEducativos = new HashSet<CentrosEducativo>();
            Municipios = new HashSet<Municipio>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<CentrosEducativo> CentrosEducativos { get; set; }
        public virtual ICollection<Municipio> Municipios { get; set; }
    }
}
