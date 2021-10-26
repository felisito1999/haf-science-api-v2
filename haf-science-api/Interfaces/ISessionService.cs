using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface ISessionService<T, TPaginatedView>
        where T : class
        where TPaginatedView : class
    {
        Task<T> GetById(int id);
        Task<T> GetTeacherSessionsById(int id, int teacherId);
        Task Save(SessionSaveModel session, int userId);
        Task Update(T session);
        Task Delete(int id);
        public Task<IEnumerable<TPaginatedView>> GetPaginatedSessions(int page, int pageSize);
        public Task<int> GetPaginatedSessionsCount();
        public Task<IEnumerable<TPaginatedView>> GetPaginatedSessionsBy(int page, int pageSize,
             string name, int? centroEducativoId);
        public Task<int> GetPaginatedSessionsCountBy(string name, int? centroEducativo);
        public Task<IEnumerable<PaginatedSesionesView>> GetPaginatedTeacherSessions(int page, int pageSize, int teacherId);
        public Task<int> GetPaginatedTeacherSessionsCount(int teacherId);
        public Task<IEnumerable<TPaginatedView>> GetPaginatedTeacherSessionsDataBy(int page, int pageSize,
             int? teacherId, string name);
        public Task<int> GetPaginatedTeacherSessionsCountBy(int teacherId, string name);
        public Task<IEnumerable<TPaginatedView>> GetPaginatedStudentSessionsDataBy(int page, int pageSize,
             int studentId);
        public Task<int> GetPaginatedStudentSessionsCountBy(int studentId);
    }
}
