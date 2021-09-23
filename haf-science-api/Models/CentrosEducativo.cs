using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class CentrosEducativo
    {
        public CentrosEducativo()
        {
            UsuariosDetalles = new HashSet<UsuariosDetalle>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int EstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<UsuariosDetalle> UsuariosDetalles { get; set; }
    }
}
