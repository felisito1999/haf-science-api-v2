using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreCentroEducativo { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombreRol { get; set; }
    }
}
