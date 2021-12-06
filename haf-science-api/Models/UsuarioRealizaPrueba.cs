using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class UsuarioRealizaPrueba
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PruebaDiagnosticaId { get; set; }
        public int SessionId { get; set; }
        public string NombreUsuario { get; set; }
        public string TituloPrueba { get; set; }
        public decimal Calificacion { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public bool IntentoCompletado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual PruebasDiagnostica PruebaDiagnostica { get; set; }
        public virtual Sesione Session { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
