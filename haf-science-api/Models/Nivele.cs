using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Nivele
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string RecompensaNivel { get; set; }
        public int JuegoId { get; set; }
        public int EstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual Estado Estado { get; set; }
        public virtual Juego Juego { get; set; }
    }
}
