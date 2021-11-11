using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface ICentrosEducativosService<T, TPaginatedView>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int? id);
        Task<IEnumerable<T>> GetByName(string name);
        Task SaveSingle(T centroEducativo);
        Task SaveMultiple(IEnumerable<T> centrosEducativos);
        Task Update(T centroEducativo);
        Task Delete(int id);
        public Task<IEnumerable<TPaginatedView>> GetPaginatedData(int page, int pageSize);
        public Task<int> GetPaginatedUsersCount();
        public Task<IEnumerable<TPaginatedView>> GetPaginatedDataBy(int page, int pageSize,
             string name);
        public Task<int> GetPaginatedUsersCountBy(string name);
        public Task<IEnumerable<TiposCentrosEducativo>> GetAllTiposCentrosEducativos();
        public Task<IEnumerable<Regionale>> GetAllRegionales();
        public Task<IEnumerable<Distrito>> GetAllDistritos();
        public Task<IEnumerable<Distrito>> GetDistritosByRegionalId(int regionalId);
    }
}
