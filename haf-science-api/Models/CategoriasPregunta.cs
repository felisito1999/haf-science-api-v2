using System;
using System.Collections.Generic;

#nullable disable

namespace haf_science_api.Models
{
    public partial class CategoriasPregunta
    {
        public CategoriasPregunta()
        {
            Pregunta = new HashSet<Pregunta>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Pregunta> Pregunta { get; set; }
    }
}
