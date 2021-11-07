using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Respuesta
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public bool EsCorrecta { get; set; }
        public int PreguntaId { get; set; }
        public int EstadoId { get; set; }
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }
}
