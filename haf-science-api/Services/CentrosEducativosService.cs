using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class CentrosEducativosService : IDataService<CentrosEducativo>
    {
        private readonly HafScienceDbContext _dbContext;
        private IMapper _mapper;
        public CentrosEducativosService(HafScienceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CentrosEducativo>> GetAll()
        {
            var centrosEducativos = await _dbContext.CentrosEducativos.ToListAsync();

            return centrosEducativos; 
        }
        public async Task<CentrosEducativo> GetById(int? id)
        {
            try
            {
                var centroEducativo = await _dbContext.CentrosEducativos.Where(x => x.Id == id).FirstOrDefaultAsync();

                return centroEducativo;
            }
            catch (Exception)
            {

                throw ;
            }
        }
        public Task SaveSingle(CentrosEducativo dataObject)
        {
            throw new NotImplementedException();
        }
        public Task SaveMultiple(IEnumerable<CentrosEducativo> dataCollection)
        {
            throw new NotImplementedException();
        }
        public Task Update(CentrosEducativo dataObject)
        {
            throw new NotImplementedException();
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CentrosEducativo>> GetByName(string name)
        {
            try
            {
                var centrosEducativos = await _dbContext.CentrosEducativos.FromSqlRaw("EXECUTE spGetSchoolsByName {0}", name).ToListAsync();

                return centrosEducativos;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
