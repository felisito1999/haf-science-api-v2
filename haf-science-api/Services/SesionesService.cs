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
    public class SesionesService : IDataService<SesionesModel, PaginatedSesionesView>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public SesionesService(HafScienceDbContext dbContext, ILogger<SesionesService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SesionesModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SesionesModel> GetById(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SesionesModel>> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PaginatedSesionesView>> GetPaginatedData(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PaginatedSesionesView>> GetPaginatedDataBy(int page, int pageSize, string name)
        {
            if (userId.HasValue)
            {

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var sesiones = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetAllPaginatedSessionsByName {0}, {1}, {2}", page, pageSize, name)
                        .ToListAsync();

                    return sesiones;
                }
            }
        }

        public Task<int> GetPaginatedUsersCount()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetPaginatedUsersCountBy(string name)
        {
            throw new NotImplementedException();
        }

        public Task SaveMultiple(IEnumerable<SesionesModel> dataCollection)
        {
            throw new NotImplementedException();
        }

        public Task SaveSingle(SesionesModel dataObject)
        {
            throw new NotImplementedException();
        }

        public Task Update(SesionesModel dataObject)
        {
            throw new NotImplementedException();
        }
    }
}
