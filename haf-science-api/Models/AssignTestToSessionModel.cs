﻿using System;

namespace haf_science_api.Models
{
    public class AssignTestToSessionModel
    {
        public int SessionId { get; set; }
        public int PruebaDiagnosticaId { get; set; }
        public int DuracionMinutos { get; set; }
        public int CantidadIntentos { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaLimite { get; set; }
    }
}
