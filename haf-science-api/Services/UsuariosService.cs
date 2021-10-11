using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class UsuariosService : IUserService<UsuariosModel>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public UsuariosService(HafScienceDbContext dbContext, IPasswordService passwordService, IMapper mapper, ILogger<UsuariosService> logger)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _mapper = mapper;
            _logger = logger; 
        }
        public async Task<UsuariosModel> GetUsuarioLoginInfo(string username, string password)
        {
            try
            {
                var user = await GetUsuarioByUsername(username);

                if (user != null)
                {
                    var byteSalt = _passwordService.ConvertStringSaltToByteArray(user.Salt);
                    var hashedPassword = _passwordService.HashPassword(password, byteSalt);

                    if (user.Contrasena != hashedPassword)
                    {
                        user = null;
                        return user;
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        public async Task<UsuariosModel> GetUsuarioByUsername(string username)
        {
            try
            {
                var usuario = (await _dbContext.UsuariosModel.FromSqlRaw("EXECUTE spGetUserDataByUsername {0}", username).ToListAsync()).FirstOrDefault();

                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        public async Task Update(UsuariosModel user)
        {
            try
            {
                var parameters = new object[]
                {
                    user.Id,
                    user.Nombres,
                    user.Apellidos,
                    user.FechaNacimiento,
                    user.Telefono,
                    user.CorreoElectronico,
                    user.NombreUsuario,
                    user.RolId,
                    user.EstadoId,
                    user.CentroEducativoId
                };

                await _dbContext.Database
                    .ExecuteSqlRawAsync("spUpdateUserData {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
                    parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        public async Task Delete(int id)
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync("spDeleteUser {0}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        public async Task Register(UsuariosModel user)
        {
            try
            {
                byte[] salt = _passwordService.GetSalt();
                string stringSalt = _passwordService.ConvertSaltToString(salt);
                string newPassword = _passwordService.CreateDefaultUserPassword(user.Nombres, user.Apellidos, user.FechaNacimiento);
                string hashedNewPassword = _passwordService.HashPassword(newPassword, salt);

                var parameters = new object[]
                {
                user.Nombres,
                user.Apellidos,
                user.FechaNacimiento,
                user.Telefono,
                user.CorreoElectronico,
                user.NombreUsuario,
                stringSalt,
                hashedNewPassword,
                user.RolId,
                user.CentroEducativoId,
                user.CreadoPor
                };

                await _dbContext.Database
                    .ExecuteSqlRawAsync("spRegisterUser {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
                    parameters);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<UsuariosModel>> GetUsers()
        {
            try
            {
                var users = await _dbContext.UsuariosModel.FromSqlRaw("g").ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<UsuariosModel> GetUsuarioById(int id)
        {
            try
            {
                var user = (await _dbContext.UsuariosModel
                    .FromSqlRaw("EXECUTE spGetUserDataById {0}", id)
                    .ToListAsync())
                    .FirstOrDefault();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PaginatedUsuariosView>> GetPaginatedUsers(int page, int pageSize)
        {
            try
            {
                var users = await _dbContext.PaginatedUsuariosView
                    .FromSqlRaw("EXECUTE spGetAllPaginatedUsersData {0}, {1}", page, pageSize)
                    .ToListAsync();

                return users; 
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
                var count = (await _dbContext.TotalRecordsModel
                    .FromSqlRaw("EXECUTE spGetAllPaginatedUsersDataCount")
                    .ToListAsync()).FirstOrDefault();

                return count.RecordsTotal;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PaginatedUsuariosView>> GetPaginatedUsersBy(int page, int pageSize, 
            int? centroEducativoId, string username, string name, string correoElectronico, int? rolId)
        {
            try
            {
                //Por nombre, rol y centro educativo
                if (!string.IsNullOrWhiteSpace(name) && rolId.HasValue && centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var userByNameRolAndSchool = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedUserDataByNameRoleSchool {0}, {1}, {2}, {3}, {4}", page, pageSize, name, rolId, centroEducativoId)
                        .ToListAsync();

                    return userByNameRolAndSchool;
                }
                //Por nombre y rol
                else if (!string.IsNullOrWhiteSpace(name) && rolId.HasValue && !centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var usersByNameAndRoleId = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedUsersDataByNameRole {0}, {1}, {2}, {3}", page, pageSize, name, rolId)
                        .ToListAsync();

                    return usersByNameAndRoleId;
                }
                //Por nombre y centro educativo 
                else if (!string.IsNullOrWhiteSpace(name) && !rolId.HasValue && centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var usersByNameAndSchools = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedUsersDataByNameSchools {0}, {1}, {2}, {3}", page, pageSize, name, centroEducativoId)
                        .ToListAsync();

                    return usersByNameAndSchools;
                }
                //Por centro educativo y rol
                else if (centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(name)
                    && string.IsNullOrWhiteSpace(correoElectronico) && rolId.HasValue)
                {
                    var usersBySchoolsAndRol = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedUserDataBySchoolsRoles {0}, {1}, {2}, {3}", page, pageSize, centroEducativoId, rolId)
                        .ToListAsync();

                    return usersBySchoolsAndRol;
                }
                //Por nombre
                else if (!string.IsNullOrWhiteSpace(name) && !centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico) && !rolId.HasValue)
                {
                    var usersByName = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedUsersDataByName {0}, {1}, {2}", page, pageSize, name)
                        .ToListAsync();

                    return usersByName;
                }
                //Por centrosEducativos
                else if (centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(name)
                    && string.IsNullOrWhiteSpace(correoElectronico) && !rolId.HasValue)
                {
                    var usersByCentroEducativoId = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedUserDataBySchools {0}, {1}, {2}", page, pageSize, centroEducativoId)
                        .ToListAsync();

                    return usersByCentroEducativoId;
                }
                //Por rol
                else if (rolId.HasValue && !centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var usersByRolId = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedUserDataByRole {0}, {1}, {2}", page, pageSize, rolId)
                        .ToListAsync();

                    return usersByRolId;
                }


                return await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetAllPaginatedUsersData {0}, {1}", page, pageSize)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedUsersCountBy(int? centroEducativoId, string username, 
            string name, string correoElectronico, int? rolId)
        {
            try
            {
                //Por nombre, rol y centros educativo
                if (!string.IsNullOrWhiteSpace(name) && rolId.HasValue && centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var userByNameRolAndSchool = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedUserDataByNameRolSchoolCount {0}, {1}, {2}", name, rolId, centroEducativoId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return userByNameRolAndSchool;
                }
                //Por nombre y rol
                else if (!string.IsNullOrWhiteSpace(name) && rolId.HasValue && !centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var usersByNameAndRole = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedUsersDataByNameRoleCount {0}, {1}", name, rolId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return usersByNameAndRole;
                }
                //Por nombre y centro educativo 
                else if (!string.IsNullOrWhiteSpace(name) && !rolId.HasValue && centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var usersByNameAndSchools = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedUsersDataByNameSchoolsCount {0}, {1}", name, centroEducativoId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return usersByNameAndSchools;
                }
                //Por centro educativo y rol 
                else if (centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(name)
                    && string.IsNullOrWhiteSpace(correoElectronico) && rolId.HasValue)
                {
                    var userBySchoolsAndRoles = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedUserDataBySchoolsRolesCount {0}, {1}", centroEducativoId, rolId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return userBySchoolsAndRoles;
                }
                //Por nombre
                else if (!string.IsNullOrWhiteSpace(name) && !centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(correoElectronico) && !rolId.HasValue)
                {
                    var usersByName = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("Execute spGetPaginatedUsersByNameDataCount {0}", name)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return usersByName;
                }
                //Por centros educativos
                else if (centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(name)
                    && string.IsNullOrWhiteSpace(correoElectronico) && !rolId.HasValue)
                {
                    var usersByCentroEducativoId = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedUsersDataBySchoolsCount {0}", centroEducativoId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return usersByCentroEducativoId;
                }
                //Por rol
                else if (rolId.HasValue && !centroEducativoId.HasValue && string.IsNullOrWhiteSpace(username)
                    && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(correoElectronico))
                {
                    var usersByRolId = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedUsersDataByRoleCount {0}", rolId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return usersByRolId;
                }

                return (await _dbContext.TotalRecordsModel.FromSqlRaw("EXECUTE spGetAllPaginatedUsersDataCount").ToListAsync()).FirstOrDefault().RecordsTotal;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
