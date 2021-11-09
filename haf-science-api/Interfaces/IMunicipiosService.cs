using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IMunicipiosService<T>
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<IEnumerable<T>> GetMuncipiosByProvinciaId(int id);
    }
}
