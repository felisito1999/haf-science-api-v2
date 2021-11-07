using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Juego
    {
        public Juego()
        {
            Niveles = new HashSet<Nivele>();
        }

        public int Id { get; set; }
        public string NombreJuego { get; set; }
        public string ReglasJuego { get; set; }
        public string DescripcionJuego { get; set; }
        public string RecompensaJuego { get; set; }
        public bool Eliminado { get; set; }
        public int EstadoId { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual ICollection<Nivele> Niveles { get; set; }
    }
}
