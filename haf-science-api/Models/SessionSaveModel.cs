﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Models
{
    public class SessionSaveModel
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IEnumerable<SessionStudents> UsuariosSesiones { get; set; }
    }
}
