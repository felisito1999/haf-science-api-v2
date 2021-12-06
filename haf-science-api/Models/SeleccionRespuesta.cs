using System.Collections.Generic;

namespace haf_science_api.Models
{
    public class SeleccionRespuesta
    {
        public int PreguntaId { get; set; }
        public string TituloPregunta { get; set; }
        public IEnumerable<AttemptRespuestas> Respuestas { get; set; }
    }
}
