using AutoMapper;
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
    public class ProvinciasService : IProvinciasService<Provincia>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;
        //private readonly IMapper _mapper;
        public ProvinciasService(HafScienceDbContext dbContext, ILogger<ProvinciasService> logger/*, IMapper mapper*/)
        {
            _dbContext = dbContext;
            _logger = logger;
            //_mapper = mapper; 
        }
        public async Task<IEnumerable<Provincia>> GetAll()
        {
            try
            {
                return await _dbContext.Provincias.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
