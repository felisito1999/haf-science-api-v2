using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class EstadosService : IDataService<Estado>
    {
        private readonly HafScienceDbContext _dbContext;

        public EstadosService(HafScienceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Estado>> GetAll()
        {
            var estados = await _dbContext.Estados
                .FromSqlRaw("EXECUTE spGetEstados").ToListAsync();
            return estados;
        }

        public async Task<Estado> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveMultiple(IEnumerable<Estado> dataCollection)
        {
            throw new NotImplementedException();
        }

        public async Task SaveSingle(Estado dataObject)
        {
            throw new NotImplementedException();
        }

        public async Task Update(Estado dataObject)
        {
            throw new NotImplementedException();
        }
    }
}
