using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    interface ISessionService<T, TPaginatedView>
        where T : class
        where TPaginatedView : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int? id);
        Task<IEnumerable<T>> GetByName(string name);
        Task SaveSingle(T session);
        Task SaveMultiple(IEnumerable<T> sessionCollection);
        Task Update(T session);
        Task Delete(int id);
        public Task<IEnumerable<TPaginatedView>> GetPaginatedSessions(int page, int pageSize);
        public Task<int> GetPaginatedSessionsCount();
        public Task<IEnumerable<TPaginatedView>> GetPaginatedSessionsBy(int page, int pageSize,
             string name, int? centroEducativoId);
        public Task<int> GetPaginatedSessionsCountBy(string name, int? centroEducativo);
        public Task<IEnumerable<TPaginatedView>> GetPaginatedUserSessionsDataBy(int page, int pageSize,
             int teacherId);
        public Task<int> GetPaginatedUserSessionsCountBy(int teacherId);
    }
}
