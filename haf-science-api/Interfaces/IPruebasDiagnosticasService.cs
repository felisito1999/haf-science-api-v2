using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IPruebasDiagnosticasService<T> where T : class
    {
        Task<PruebasDiagnostica> GetPruebaDiagnosticaById(int pruebaDiagnosticaId);
        Task<object> GetSessionPruebaDiagnosticaById(int pruebaDiagnosticaId, int sessionId);
        Task AssignToSession(AssignTestToSessionModel assignToSessionModel, int teacherId);
        Task<IEnumerable<T>> GetTeacherPaginatedPruebasDiagnosticas(int teacherId, int page, int pageSize);
        Task<int> GetTeacherPaginatedPruebasDiagnosticasCount(int teacherId);
        Task<IEnumerable<object>> GetPaginatedPruebasDiagnosticasBySessionId(int sessionId, int page, int pageSize);
        Task<int> GetPaginatedPruebasDiagnosticasBySessionIdCount(int sessionId);
        Task<IEnumerable<object>> GetPaginatedStudentPruebasDiagnosticasBySessionId(int studentId, int sessionId, int page, int pageSize);
        Task<int> GetPaginatedStudentPruebasDiagnosticasBySessionIdCount(int sessionId);
        Task SavePruebaDiagnostica(PruebasDiagnosticasModel pruebaDiagnostica, int teacherId);
        Task<object> IsAvailableForStudent(int studentId, int sessionId, int pruebaDiagnosticaId);
        Task StartTest(int pruebaDiagnosticaId, UsuariosModel student, int sessionId);
        Task EvaluateTestGrade(AttemptModel attempt, int sessionId, int studentId);
        Task<object> GetSessionAverageGrades(int sessionId, int teacherId);
        Task<object> GetSessionTestAverageGrades(int sessionId, int testId, int teacherId);
        Task<object> GetStudentSessionAverageGrades(int sessionId, int studentId, int teacherId);
        Task<IEnumerable<object>> GetTestSessionGrades(int testId, int sessionId, int teacherId);
    }
}
