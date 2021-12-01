using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IPruebasDiagnosticasService<T> where T : class
    {
        Task AssignToSession(AssignTestToSessionModel assignToSessionModel, int teacherId);
        Task<IEnumerable<T>> GetTeacherPaginatedPruebasDiagnosticas(int teacherId, int page, int pageSize);
        Task<int> GetTeacherPaginatedPruebasDiagnosticasCount(int teacherId);
        Task<IEnumerable<object>> GetPaginatedPruebasDiagnosticasBySessionId(int sessionId, int page, int pageSize);
        Task<int> GetPaginatedPruebasDiagnosticasBySessionIdCount(int sessionId);
        Task SavePruebaDiagnostica(PruebasDiagnosticasModel pruebaDiagnostica, int teacherId);
    }
}
