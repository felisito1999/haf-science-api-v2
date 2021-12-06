using System.Collections.Generic;

namespace haf_science_api.Models
{
    public class AttemptModel
    {
        public int PruebaId { get; set; }
        public int SessionId { get; set; }
        public IEnumerable<SeleccionRespuesta> Preguntas{ get; set; }
    }
}
