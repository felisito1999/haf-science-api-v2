using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class PruebasDiagnostica
    {
        public PruebasDiagnostica()
        {
            PruebasPregunta = new HashSet<PruebasPregunta>();
            PruebasSesiones = new HashSet<PruebasSesione>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public int CalificacionMaxima { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaLimite { get; set; }
        public int EstadoId { get; set; }
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ICollection<PruebasPregunta> PruebasPregunta { get; set; }
        public virtual ICollection<PruebasSesione> PruebasSesiones { get; set; }
    }
}
