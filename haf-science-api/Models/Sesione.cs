using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class Sesione
    {
        public Sesione()
        {
            PruebasSesiones = new HashSet<PruebasSesione>();
            UsuarioRealizaPruebas = new HashSet<UsuarioRealizaPrueba>();
            UsuariosSesiones = new HashSet<UsuariosSesione>();
            UsuariosSesionesInsignia = new HashSet<UsuariosSesionesInsignia>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CentroEducativoId { get; set; }
        public int CreadoPor { get; set; }
        public int EstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual CentrosEducativo CentroEducativo { get; set; }
        public virtual Usuario CreadoPorNavigation { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ICollection<PruebasSesione> PruebasSesiones { get; set; }
        public virtual ICollection<UsuarioRealizaPrueba> UsuarioRealizaPruebas { get; set; }
        public virtual ICollection<UsuariosSesione> UsuariosSesiones { get; set; }
        public virtual ICollection<UsuariosSesionesInsignia> UsuariosSesionesInsignia { get; set; }
    }
}
