using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Insignia
    {
        public Insignia()
        {
            UsuariosSesionesInsignia = new HashSet<UsuariosSesionesInsignia>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TipoInsigniaId { get; set; }
        public int ImagenesInsignias { get; set; }
        public int EstadoId { get; set; }
        public int CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Eliminado { get; set; }

        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ImagenesInsignia ImagenesInsigniasNavigation { get; set; }
        public virtual TiposInsignia TipoInsignia { get; set; }
        public virtual ICollection<UsuariosSesionesInsignia> UsuariosSesionesInsignia { get; set; }
    }
}
