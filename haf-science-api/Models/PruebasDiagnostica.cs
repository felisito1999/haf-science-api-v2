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
            UsuarioRealizaPruebas = new HashSet<UsuarioRealizaPrueba>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public decimal CalificacionMaxima { get; set; }
        public int EstadoId { get; set; }
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ICollection<PruebasPregunta> PruebasPregunta { get; set; }
        public virtual ICollection<PruebasSesione> PruebasSesiones { get; set; }
        public virtual ICollection<UsuarioRealizaPrueba> UsuarioRealizaPruebas { get; set; }
    }
}
