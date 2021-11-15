using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class CentrosEducativosModel
    {
        public int Id { get; set; }
        public string CodigoCentro { get; set; }
        public int DirectorId { get; set; }
        public string DirectorNombreCompleto { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int ProvinciaId { get; set; }
        public string ProvinciaNombre { get; set; }
        public int MunicipioId { get; set; }
        public string MunicipioNombre { get; set; }
        public int RegionalId { get; set; }
        public string RegionalNombre { get; set; }
        public int DistritoId { get; set; }
        public string DistritoNombre { get; set; }
        public int TipoCentroEducativoId { get; set; }
        public string TipoCentroEducativoNombre { get; set; } 
        public string NombreEstado { get; set; }
    }
}
