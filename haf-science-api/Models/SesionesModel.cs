using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class SesionesModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CentroEducativoId { get; set; }
        public string NombreCentroEducativo { get; set; }
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }
        public int CreadoPor { get; set; }
    }
}
