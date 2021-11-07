using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Pregunta
    {
        public Pregunta()
        {
            PruebasPregunta = new HashSet<PruebasPregunta>();
            Respuesta = new HashSet<Respuesta>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal? Valoracion { get; set; }
        public int CategoriaPreguntaId { get; set; }
        public int EstadoId { get; set; }
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual CategoriasPregunta CategoriaPregunta { get; set; }
        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ICollection<PruebasPregunta> PruebasPregunta { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }
    }
}
