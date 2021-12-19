using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class UsuariosSesionesInsignia
    {
        public int UsuarioId { get; set; }
        public int SesionId { get; set; }
        public int InsigniaId { get; set; }
        public bool IsFavorite { get; set; }

        public virtual Insignia Insignia { get; set; }
        public virtual Sesione Sesion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
