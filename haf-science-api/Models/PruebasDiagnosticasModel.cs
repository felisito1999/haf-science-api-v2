using System.Collections.Generic;

namespace haf_science_api.Models
{
    public class PruebasDiagnosticasModel
    {
        public PruebasDiagnostica PruebaDiagnostica { get; set; }
        public IEnumerable<Pregunta> Preguntas{ get; set; }
    }
}
