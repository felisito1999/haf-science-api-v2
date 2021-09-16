using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IDataService<T>
        where T : class
    {
        Task<IEnumerable<T>> GetData();
        Task<T> GetDataById(int id);
        Task SaveSingle(T dataObject);
        Task SaveMultiple(IEnumerable<T> dataCollection);
        Task Update(T dataObject);
        Task Delete(int id);
    }
}
