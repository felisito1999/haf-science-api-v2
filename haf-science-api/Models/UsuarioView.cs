using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    [Keyless]
    public class UsuarioView
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public int CentroEducativoId { get; set; }
        public string NombreCentroEducativo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string NombreUsuario { get; set; }
        public int RolId { get; set; }
        public string NombreRol { get; set; }
        public int EstadoId { get; set; }
        public string NombreEstado { get; set; }
        public int? CreadoPor { get; set; }
        public string NombreUsuarioCreador { get; set; }
    }
}
