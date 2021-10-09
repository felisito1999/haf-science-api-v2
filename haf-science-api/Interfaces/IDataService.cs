using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IDataService<T, TPaginatedView>
        where T : class
        where TPaginatedView : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int? id);
        Task<IEnumerable<T>> GetByName(string name);
        Task SaveSingle(T dataObject);
        Task SaveMultiple(IEnumerable<T> dataCollection);
        Task Update(T dataObject);
        Task Delete(int id);
        public Task<IEnumerable<TPaginatedView>> GetPaginatedData(int page, int pageSize);
        public Task<int> GetPaginatedUsersCount();
        public Task<IEnumerable<TPaginatedView>> GetPaginatedDataBy(int page, int pageSize,
             string name);
        public Task<int> GetPaginatedUsersCountBy(string name);
    }
}
