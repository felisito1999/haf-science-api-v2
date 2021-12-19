using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Estado
    {
        public Estado()
        {
            CentrosEducativos = new HashSet<CentrosEducativo>();
            Insignia = new HashSet<Insignia>();
            Juegos = new HashSet<Juego>();
            Niveles = new HashSet<Nivele>();
            Pregunta = new HashSet<Pregunta>();
            PruebasDiagnosticas = new HashSet<PruebasDiagnostica>();
            Respuesta = new HashSet<Respuesta>();
            Sesiones = new HashSet<Sesione>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int TipoEstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual TiposEstado TipoEstado { get; set; }
        public virtual ICollection<CentrosEducativo> CentrosEducativos { get; set; }
        public virtual ICollection<Insignia> Insignia { get; set; }
        public virtual ICollection<Juego> Juegos { get; set; }
        public virtual ICollection<Nivele> Niveles { get; set; }
        public virtual ICollection<Pregunta> Pregunta { get; set; }
        public virtual ICollection<PruebasDiagnostica> PruebasDiagnosticas { get; set; }
        public virtual ICollection<Respuesta> Respuesta { get; set; }
        public virtual ICollection<Sesione> Sesiones { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
