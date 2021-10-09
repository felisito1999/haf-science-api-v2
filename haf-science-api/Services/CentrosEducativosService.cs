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
    public class CentrosEducativosService : IDataService<CentrosEducativo, PaginatedCentrosEducativosView>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;
        private IMapper _mapper;
        public CentrosEducativosService(HafScienceDbContext dbContext, IMapper mapper, ILogger<CentrosEducativosService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
                var centrosEducativos = (await _dbContext.CentrosEducativos
                    .FromSqlRaw("EXECUTE spGetPaginatedSchoolsById {0}", id)
                    .ToListAsync())
                    .FirstOrDefault();

                return centrosEducativos;
            }
            catch (Exception)
            {

                throw ;
            }
        }
        public async Task SaveSingle(CentrosEducativo centroEducativo)
        {
            try
            {
                await _dbContext.Database
                    .ExecuteSqlRawAsync("spAddSchools {0}, {1}", centroEducativo.Nombre, centroEducativo.Direccion);        
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
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
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<PaginatedCentrosEducativosView>> GetPaginatedData(int page, int pageSize)
        {
            var schools = await _dbContext.PaginatedCentrosEducativosView
                .FromSqlRaw("EXECUTE spGetPaginatedSchoolsData {0}, {1}", page, pageSize)
                .ToListAsync();

            return schools;
        }

        public async Task<int> GetPaginatedUsersCount()
        {
            var totalRecords = (await _dbContext.TotalRecordsModel
                .FromSqlRaw("spGetPaginatedSchoolsDataCount")
                .ToListAsync())
                .FirstOrDefault()
                .RecordsTotal;

            return totalRecords;
        }

        public async Task<IEnumerable<PaginatedCentrosEducativosView>> GetPaginatedDataBy(int page, int pageSize,
             string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var centrosEducativos = await _dbContext.PaginatedCentrosEducativosView
                .FromSqlRaw("EXECUTE spGetPaginatedSchoolsByName {0}, {1}, {2}", page, pageSize, name)
                .ToListAsync();

                return centrosEducativos;
            }
            else
            {
                var centrosEducativos = await GetPaginatedData(page, pageSize);

                return centrosEducativos;
            }
        }

        public async Task<int> GetPaginatedUsersCountBy(string name)
        {
            var totalRecords = (await _dbContext.TotalRecordsModel
                .FromSqlRaw("EXECUTE spGetPaginatedSchoolByNameCount {0}", name)
                .ToListAsync())
                .FirstOrDefault()
                .RecordsTotal;

            return totalRecords;
        }
    }
}
