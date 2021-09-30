using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class RolesService : IDataService<Role>
    {
        private readonly HafScienceDbContext _dbContext;
        public RolesService(HafScienceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            try
            {
                var roles = await _dbContext.Roles.ToListAsync();

                return roles;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<Role> GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetByName(string name)
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
