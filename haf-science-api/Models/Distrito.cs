using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Distrito
    {
        public Distrito()
        {
            CentrosEducativos = new HashSet<CentrosEducativo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int RegionalId { get; set; }

        public virtual Regionale Regional { get; set; }
        public virtual ICollection<CentrosEducativo> CentrosEducativos { get; set; }
    }
}
