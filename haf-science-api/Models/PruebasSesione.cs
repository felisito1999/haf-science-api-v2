using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class PruebasSesione
    {
        public int PruebaDiagnosticaId { get; set; }
        public int SesionId { get; set; }
        public string NombreSesion { get; set; }
        public string TituloPrueba { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual PruebasDiagnostica PruebaDiagnostica { get; set; }
        public virtual Sesione Sesion { get; set; }
    }
}
