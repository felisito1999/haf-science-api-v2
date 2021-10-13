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
    public class CentrosEducativosService : IDataService<CentrosEducativosModel, PaginatedCentrosEducativosView>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public CentrosEducativosService(HafScienceDbContext dbContext, IMapper mapper, ILogger<CentrosEducativosService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CentrosEducativosModel>> GetAll()
        {
            try
            {
                var centrosEducativos = await _dbContext.CentrosEducativosModel.ToListAsync();

                return centrosEducativos;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        public async Task<CentrosEducativosModel> GetById(int? id)
        {
            try
            {
                var centrosEducativos = (await _dbContext.CentrosEducativosModel
                    .FromSqlRaw("EXECUTE spGetSchoolsDataById {0}", id)
                    .ToListAsync())
                    .FirstOrDefault();

                return centrosEducativos;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw ;
            }
        }
        public async Task SaveSingle(CentrosEducativosModel centroEducativo)
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
        public Task SaveMultiple(IEnumerable<CentrosEducativosModel> dataCollection)
        {
            throw new NotImplementedException();
        }
        public async Task Update(CentrosEducativosModel dataObject)
        {
            try
            {
                if (dataObject != null)
                {
                    await _dbContext.Database.ExecuteSqlRawAsync("spUpdateSchools {0}, {1}, {2}", dataObject.Id, dataObject.Nombre, dataObject.Direccion);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync("spDeleteSchools {0}", id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<CentrosEducativosModel>> GetByName(string name)
        {
            try
            {
                var centrosEducativos = await _dbContext.CentrosEducativosSelectModel.FromSqlRaw("EXECUTE spGetSchoolsByName {0}", name).ToListAsync();

                return _mapper.Map<List<CentrosEducativosSelectModel>, IEnumerable<CentrosEducativosModel>>(centrosEducativos);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PaginatedCentrosEducativosView>> GetPaginatedData(int page, int pageSize)
        {
            try
            {
                var schools = await _dbContext.PaginatedCentrosEducativosView
                .FromSqlRaw("EXECUTE spGetPaginatedSchoolsData {0}, {1}", page, pageSize)
                .ToListAsync();

                return schools;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedUsersCount()
        {
            try
            {
                var totalRecords = (await _dbContext.TotalRecordsModel
                .FromSqlRaw("spGetPaginatedSchoolsDataCount")
                .ToListAsync())
                .FirstOrDefault()
                .RecordsTotal;

                return totalRecords;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PaginatedCentrosEducativosView>> GetPaginatedDataBy(int page, int pageSize,
             string name)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedUsersCountBy(string name)
        {
            try
            {
                var totalRecords = (await _dbContext.TotalRecordsModel
                .FromSqlRaw("EXECUTE spGetPaginatedSchoolByNameCount {0}", name)
                .ToListAsync())
                .FirstOrDefault()
                .RecordsTotal;

                return totalRecords;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
