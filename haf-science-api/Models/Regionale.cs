using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Regionale
    {
        public Regionale()
        {
            CentrosEducativos = new HashSet<CentrosEducativo>();
            Distritos = new HashSet<Distrito>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<CentrosEducativo> CentrosEducativos { get; set; }
        public virtual ICollection<Distrito> Distritos { get; set; }
    }
}
