using haf_science_api.Interfaces;
using haf_science_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class RolesService : IDataService<Role>
    {
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task SaveMultiple(IEnumerable<Role> dataCollection)
        {
            throw new NotImplementedException();
        }

        public Task SaveSingle(Role dataObject)
        {
            throw new NotImplementedException();
        }

        public Task Update(Role dataObject)
        {
            throw new NotImplementedException();
        }
    }
}
