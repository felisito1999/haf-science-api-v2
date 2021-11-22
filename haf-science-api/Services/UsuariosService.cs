using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class UsuariosService : IUserService<UsuariosModel>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ILogger _logger;
        public UsuariosService(HafScienceDbContext dbContext, IPasswordService passwordService, ILogger<UsuariosService> logger)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _logger = logger; 
        }
        public async Task<UsuariosModel> GetUsuarioByUsernameAndPassword(string username, string password)
        {
            try
            {
                var user = (await _dbContext.UsuariosModel
                    .FromSqlRaw("EXECUTE spGetUserByUsernamePassword {0}, {1}", username, password)
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

        public async Task<UsuariosModel> GetUsuarioLoginInfo(string username, string password)
        {
            try
            {
                var user = await GetUsuarioByUsername(username);

                if (user != null)
                {
                    var byteSalt = await _passwordService.ConvertStringSaltToByteArray(user.Salt);
                    var hashedPassword = await _passwordService.HashPassword(password, byteSalt);

                    var userByUsernameAndPassword = await GetUsuarioByUsernameAndPassword(username, hashedPassword);
                    if (userByUsernameAndPassword == null)
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
                    user.Contrasena,
                    user.FechaNacimiento,
                    user.Telefono,
                    user.CorreoElectronico,
                    user.NombreUsuario,
                    user.RolId,
                    user.EstadoId,
                    user.CentroEducativoId
                };

                await _dbContext.Database
                    .ExecuteSqlRawAsync("spUpdateUserData {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}",
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
                byte[] salt = await _passwordService.GetSalt();
                string stringSalt = await _passwordService.ConvertSaltToString(salt);
                string newPassword = await _passwordService.CreateDefaultUserPassword(user.Nombres, user.Apellidos, user.FechaNacimiento);
                string hashedNewPassword = await _passwordService.HashPassword(newPassword, salt);

                var parameters = new object[]
                {
                user.Nombres,
                user.Apellidos,
                user.FechaNacimiento,
                user.Telefono,
                user.CorreoElectronico,
                stringSalt,
                hashedNewPassword,
                user.RolId,
                user.EsSuperAdministrador,
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

        public async Task<IEnumerable<PaginatedUsuariosView>> GetPaginatedUsers(int page, int pageSize, int? requestingUserSchoolId)
        {
            try
            {
                var users = await _dbContext.PaginatedUsuariosView
                    .FromSqlRaw("EXECUTE spGetAllPaginatedUsersData {0}, {1}, {2}", requestingUserSchoolId, page, pageSize)
                    .ToListAsync();

                return users; 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        public async Task<int> GetPaginatedUsersCount(int? requestingUserSchoolId)
        {
            try
            {
                var count = (await _dbContext.TotalRecordsModel
                    .FromSqlRaw("EXECUTE spGetAllPaginatedUsersDataCount {0}", requestingUserSchoolId)
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
            int? centroEducativoId, string username, string name, string correoElectronico, int? rolId, int? requestingUserSchoolId)
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
                        .FromSqlRaw("EXECUTE spGetAllPaginatedUsersData {0}, {1}, {2}", page, pageSize, requestingUserSchoolId)
                        .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedUsersCountBy(int? centroEducativoId, string username, 
            string name, string correoElectronico, int? rolId, int? requestingUserSchoolId)
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

        public async Task<IEnumerable<PaginatedUsuariosView>> GetPaginatedTeacherStudents(int page, int pageSize, int teacherId)
        {
            try
            {
                var users = await _dbContext.PaginatedUsuariosView
                    .FromSqlRaw("EXECUTE spGetPaginatedTeacherStudents {0}, {1}, {2}", page, pageSize, teacherId)
                    .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedTeacherStudentsCount(int teacherId)
        {
            try
            {
                var count = (await _dbContext.TotalRecordsModel
                    .FromSqlRaw("EXECUTE spGetPaginatedTeacherStudentsCount {0}", teacherId)
                    .ToListAsync())
                    .FirstOrDefault()
                    .RecordsTotal;

                return count; 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PaginatedUsuariosView>> GetPaginatedTeacherStudentsDataBy(int page, int pageSize, int teacherId, string name, int? sessionId)
        {
            try
            {
                if (sessionId.HasValue)
                {
                    var user = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedTeacherUsersBySession {0}, {1}, {2}, {3}", page, pageSize, teacherId, sessionId)
                        .ToListAsync();

                    return user;
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    var users = await _dbContext.PaginatedUsuariosView
                        .FromSqlRaw("EXECUTE spGetPaginatedTeacherStudentsByName {0}, {1}, {2}, {3}", page, pageSize, teacherId, name)
                        .ToListAsync();

                    return users; 
                }
                else
                {
                    var users = await GetPaginatedTeacherStudents(page, pageSize, teacherId);

                    return users; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedTeacherStudentsCountBy(int teacherId, string name, int? sessionId)
        {
            try
            {
                if (sessionId.HasValue)
                {
                    var recordsTotal = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedTeacherUsersBySessionCount {0}, {1}", teacherId, sessionId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return recordsTotal;
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    var recordsTotal = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetPaginatedTeacherStudentsByNameCount {0}, {1}", teacherId, name)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return recordsTotal; 
                }
                else
                {
                    var recordsTotal = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("spGetPaginatedTeacherStudentsCount {0}", teacherId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return recordsTotal; 
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<UsuariosModel> GetTeacherStudentById(int teacherId, int studentId)
        {
            try
            {
                var student = (await _dbContext.UsuariosModel
                    .FromSqlRaw("EXECUTE spGetTeacherStudentById {0}, {1}", teacherId, studentId)
                    .ToListAsync())
                    .FirstOrDefault();

                return student;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<string> GenerateUsername(string name, string lastName, DateTime birthDate)
        {
            var username = (await _dbContext.Usuarios
                .Take(1)
                .Select(u => HafScienceDbContext.CreateUsername(name, lastName, birthDate))
                .ToListAsync())
                .FirstOrDefault();

            return username; 
        }

        public async Task<UsuariosModel> GetUsuarioByEmail(string email)
        {
            return (await _dbContext.UsuariosModel
                .FromSqlRaw("EXECUTE spGetUserDataByEmail {0}", email)
                .ToListAsync())
                .SingleOrDefault();
        }
        public async Task<string> GenerateUserHashes(int userId)
        {
            try
            {
                byte[] bytes;
                string base64Url = await Task.Run(() =>
                {
                    RandomNumberGenerator rng = RandomNumberGenerator.Create();
                    bytes = new byte[12];
                    rng.GetBytes(bytes);
                    return Convert.ToBase64String(bytes)
                    .Replace('+', '-')
                    .Replace('/', '_');
                });

                return base64Url;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task SaveUserHash(int userId, string hash)
        {
            try
            {
                UserHash hashObject = new UserHash()
                {
                    UserId = userId,
                    Hash = hash,
                    FechaExpiracion = DateTime.Now.ToUniversalTime().AddMinutes(30)
                };
                await _dbContext.UserHashes.AddAsync(hashObject);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<bool> VerifyUserHash(int userId)
        {
            var userHash = await _dbContext.UserHashes
                .Where(x => x.UserId == userId && x.FechaExpiracion >= DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (userHash == null)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                var mail = await _dbContext.UsuariosDetalles
                    .Where(x => x.CorreoElectronico == email)
                    .FirstOrDefaultAsync();   

                if (mail == null)
                {
                    return false;
                }

                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
