using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class MunicipiosService : IMunicipiosService<Municipio>
    {
        private readonly HafScienceDbContext _dbContext; 
        private readonly ILogger _logger;
        //private readonly IMapper _mapper;
        public MunicipiosService(HafScienceDbContext dbContext, ILogger<MunicipiosService> logger/*, IMapper mapper*/)
        {
            _dbContext = dbContext;
            _logger = logger; 
        }
        public async Task<IEnumerable<Municipio>> GetAll()
        {
            try
            {
                return await _dbContext.Municipios.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<Municipio>> GetMuncipiosByProvinciaId(int provinciaId)
        {
            try
            {
                return await _dbContext.Municipios.Where(x => x.ProvinciaId == provinciaId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
