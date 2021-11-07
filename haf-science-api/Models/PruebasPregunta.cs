using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class PruebasPregunta
    {
        public int PreguntaId { get; set; }
        public int PruebaId { get; set; }
        public string TituloPregunta { get; set; }
        public string TituloPrueba { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Pregunta Pregunta { get; set; }
        public virtual PruebasDiagnostica Prueba { get; set; }
    }
}
