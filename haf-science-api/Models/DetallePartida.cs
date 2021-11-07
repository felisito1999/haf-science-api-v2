using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class DetallePartida
    {
        public int Id { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public int TiempoPartida { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }
    }
}
