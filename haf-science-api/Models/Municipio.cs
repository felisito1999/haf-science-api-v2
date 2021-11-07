using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Municipio
    {
        public Municipio()
        {
            CentrosEducativos = new HashSet<CentrosEducativo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? ProvinciaId { get; set; }

        public virtual Provincia Provincia { get; set; }
        public virtual ICollection<CentrosEducativo> CentrosEducativos { get; set; }
    }
}
