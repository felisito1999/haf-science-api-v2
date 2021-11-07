using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class TiposLog
    {
        public TiposLog()
        {
            UsuariosLogs = new HashSet<UsuariosLog>();
        }

        public int Id { get; set; }
        public int Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<UsuariosLog> UsuariosLogs { get; set; }
    }
}
