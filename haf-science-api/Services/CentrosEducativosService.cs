using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
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
        public Task<IEnumerable<CentrosEducativo>> GetAll()
        {
            throw new NotImplementedException();
        }
        public Task<CentrosEducativo> GetById(int id)
        {
            throw new NotImplementedException();
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
    }
}
