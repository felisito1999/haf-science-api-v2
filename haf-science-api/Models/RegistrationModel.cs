using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class RegistrationModel
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombreUsuario { get; set; }
        public int RolId { get; set; }
        public int EstadoId { get; set; }
        public int CentroEducativoId { get; set; }
        public int CreadoPor { get; set; }
    }
}
