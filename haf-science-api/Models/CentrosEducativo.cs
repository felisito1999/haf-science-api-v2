using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class CentrosEducativo
    {
        public CentrosEducativo()
        {
            Sesiones = new HashSet<Sesione>();
            UsuariosDetalles = new HashSet<UsuariosDetalle>();
        }

        public int Id { get; set; }
        public string CodigoCentro { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int RegionalId { get; set; }
        public int DistritoId { get; set; }
        public int DirectorId { get; set; }
        public int TipoCentroEducativoId { get; set; }
        public int ProvinciaId { get; set; }
        public int MunicipioId { get; set; }
        public int EstadoId { get; set; }
        public bool Eliminado { get; set; }

        public virtual Directore Director { get; set; }
        public virtual Distrito Distrito { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Municipio Municipio { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual Regionale Regional { get; set; }
        public virtual TiposCentrosEducativo TipoCentroEducativo { get; set; }
        public virtual ICollection<Sesione> Sesiones { get; set; }
        public virtual ICollection<UsuariosDetalle> UsuariosDetalles { get; set; }
    }
}
