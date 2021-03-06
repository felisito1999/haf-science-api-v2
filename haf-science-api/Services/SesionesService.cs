using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class SesionesService : ISessionService<SesionesModel, PaginatedSesionesView>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserService<UsuariosModel> _usuariosService;
        public SesionesService(HafScienceDbContext dbContext, ILogger<SesionesService> logger, IMapper mapper, IUserService<UsuariosModel> usuariosService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _usuariosService = usuariosService; 
        }

        public async Task Delete(int id)
        {
            try
            {
                var result = await _dbContext.Database.ExecuteSqlRawAsync("spDeleteSessions {0}", id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<SesionesModel> GetById(int id)
        {
            try
            {
                var session = (await _dbContext.SesionesModel.FromSqlRaw("EXECUTE spGetSessionById {0}", id).ToListAsync()).FirstOrDefault();

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PaginatedSesionesView>> GetPaginatedSessions(int page, int pageSize)
        {
            try
            {
                var sessions = await _dbContext.PaginatedSesionesView
                    .FromSqlRaw("EXECUTE spGetAllPaginatedSessons {0}, {1}", page, pageSize)
                    .ToListAsync();

                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PaginatedSesionesView>> GetPaginatedSessionsBy(int page, int pageSize, string name, int? centroEducativoId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(name) && centroEducativoId.HasValue)
                {
                    var sessions = await _dbContext.PaginatedSesionesView
                        .FromSqlRaw("spGetPaginatedSessionsByNameAndSchool {0}, {1}, {2}, {3}", page, pageSize, name, centroEducativoId)
                        .ToListAsync();

                    return sessions;
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    var sessions = await _dbContext.PaginatedSesionesView
                        .FromSqlRaw("spGetAllPaginatedSessionsByName {0}, {1}, {2}", page, pageSize, name)
                        .ToListAsync();

                    return sessions;
                }
                else if(centroEducativoId.HasValue)
                {
                    var sessions = await _dbContext.PaginatedSesionesView
                        .FromSqlRaw("spGetPaginatedSessionsBySchools {0}, {1}, {2}", page, pageSize, centroEducativoId)
                        .ToListAsync();

                    return sessions;
                }
                else
                {
                    var sessions = await _dbContext.PaginatedSesionesView
                        .FromSqlRaw("spGetAllPaginatedSessions {0}, {1}", page, pageSize)
                        .ToListAsync();

                    return sessions;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedSessionsCount()
        {
            try
            {
                var totalRecords = (await _dbContext.TotalRecordsModel
                    .FromSqlRaw("EXECUTE spGetAllPaginatedSessionsCount")
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

        public async Task<int> GetPaginatedSessionsCountBy(string name, int? centroEducativoId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(name) && centroEducativoId.HasValue)
                {
                    var totalRecords = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("spGetPaginatedSessionsByNameAndSchoolCount {0}, {1}", name, centroEducativoId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return totalRecords;
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    var totalRecords = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("spGetAllPaginatedSessionsByNameCount {0}", name)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return totalRecords;
                }
                else if (centroEducativoId.HasValue)
                {
                    var totalRecords = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("spGetPaginatedSessionsBySchoolsCount {0}", centroEducativoId)
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return totalRecords;
                }
                else
                {
                    var totalRecords = (await _dbContext.TotalRecordsModel
                        .FromSqlRaw("EXECUTE spGetAllPaginatedSessionsCount")
                        .ToListAsync())
                        .FirstOrDefault()
                        .RecordsTotal;

                    return totalRecords;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedStudentSessionsCountBy(int studentId)
        {
            int sesionesCount = await _dbContext.Sesiones
                .Select(sesion => new {
                    sesion.Id,
                    sesion.Nombre,
                    sesion.Descripcion,
                    NombreCentroEducativo = sesion.CentroEducativo.Nombre,
                    UsuariosSesiones = sesion.UsuariosSesiones.Select(usuarioSesion => new { usuarioSesion.UsuarioId, usuarioSesion.Eliminado }).Where(usuarioSesion => usuarioSesion.UsuarioId == studentId).SingleOrDefault(),
                    sesion.FechaCreacion,
                    sesion.Eliminado
                })
                .Where(sesion => sesion.UsuariosSesiones.UsuarioId == studentId && sesion.Eliminado == false &&
                sesion.UsuariosSesiones.Eliminado == false)
                .CountAsync();

            return sesionesCount;
        }

        public async Task<IEnumerable<object>> GetPaginatedStudentSessionsDataBy(int page, int pageSize, int studentId)
        {
            int skip = (page - 1) * pageSize;

            var sesiones = await _dbContext.Sesiones
                .Select(sesion => new { 
                    sesion.Id, 
                    sesion.Nombre, 
                    sesion.Descripcion, 
                    NombreCentroEducativo = sesion.CentroEducativo.Nombre,
                    UsuariosSesiones = sesion.UsuariosSesiones.Select(usuarioSesion => new { usuarioSesion.UsuarioId, usuarioSesion.Eliminado }).Where(usuarioSesion => usuarioSesion.UsuarioId == studentId).SingleOrDefault(),
                    Profesor = $"{sesion.CreadoPorNavigation.UsuarioDetalle.Nombres} {sesion.CreadoPorNavigation.UsuarioDetalle.Apellidos}",
                    sesion.FechaCreacion, sesion.Eliminado })
                .Where(sesion => sesion.UsuariosSesiones.UsuarioId == studentId && sesion.Eliminado == false &&
                sesion.UsuariosSesiones.Eliminado == false)
                .OrderByDescending(sesion => sesion.FechaCreacion)
                .Skip(skip).Take(pageSize)
                .ToListAsync();

            return sesiones;
        }

        public async Task<IEnumerable<PaginatedSesionesView>> GetPaginatedTeacherSessions(int page, int pageSize, int teacherId)
        {
            try
            {
                var sessions = await _dbContext.PaginatedSesionesView
                    .FromSqlRaw("EXECUTE spGetPaginatedTeachersSessions {0}, {1}, {2}", page, pageSize, teacherId)
                    .ToListAsync();

                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedTeacherSessionsCountBy(int teacherId, string name)
        {
            try
            {
                var totalRecords = (await _dbContext.TotalRecordsModel
                .FromSqlRaw("EXECUTE spGetPaginatedTeachersSessionsByNameCount {0}, {1}", teacherId, name)
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

        public async Task<IEnumerable<PaginatedSesionesView>> GetPaginatedTeacherSessionsDataBy(int page, int pageSize, int? teacherId, string name)
        {
            try
            {
                var sessions = await _dbContext.PaginatedSesionesView
                    .FromSqlRaw("EXECUTE spGetPaginatedTeachersSessionsByName {0}, {1}, {2}, {3}", page, pageSize, teacherId, name)
                    .ToListAsync();

                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedTeacherSessionsCount(int teacherId)
        {
            try
            {
                var totalRecords = (await _dbContext.TotalRecordsModel
                    .FromSqlRaw("EXECUTE spGetPaginatedTeachersSessionsCount {0}", teacherId)
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

        public async Task Save(SessionSaveUpdateModel session, int userId)
        {
            try
            {
                SessionStudentsCollection sessionStudentsCollection = new SessionStudentsCollection();

                foreach(var item in session.UsuariosSesiones)
                {
                    item.NombreSesion = session.Nombre;
                    sessionStudentsCollection.Add(item);
                };

                var teacherInfo = await _usuariosService.GetUsuarioById(userId);
                
                var usersParam = new SqlParameter("UsuariosSesiones", SqlDbType.Structured)
                {
                    Value = sessionStudentsCollection,
                    TypeName = "dbo.BulkSessionStudents",
                    Direction = ParameterDirection.Input
                };
                
                var parameters = new object[]
                {
                session.Nombre,
                session.Descripcion,
                teacherInfo.CentroEducativoId,
                teacherInfo.Id,
                usersParam
                };

                var result = await _dbContext.Database.ExecuteSqlRawAsync("spSaveSessionWithStudents {0}, {1}, {2}, {3}, {4}", parameters);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        public async Task Update(SessionSaveUpdateModel session, int userId)
        {
            try
            {
                var usuariosSesiones = new SqlParameter("UsuariosSesiones", SqlDbType.Structured)
                {
                    Value = null,
                    TypeName = "dbo.BulkSessionStudents",
                    Direction = ParameterDirection.Input
                };

                if (session.UsuariosSesiones.Count() > 0)
                {
                    SessionStudentsCollection sessionStudentsCollection = new SessionStudentsCollection();
                    
                    foreach (var sessionStudent in session.UsuariosSesiones)
                    {
                        sessionStudent.SessionId = session.SessionId;
                        sessionStudent.NombreSesion = session.Nombre;
                        sessionStudentsCollection.Add(sessionStudent);
                    };

                    usuariosSesiones = new SqlParameter("UsuariosSesiones", SqlDbType.Structured)
                    {
                        Value = sessionStudentsCollection,
                        TypeName = "dbo.BulkSessionStudents",
                        Direction = ParameterDirection.Input
                    };
                }

                var parameters = new object[]
                {
                    session.SessionId,
                    userId,
                    session.Nombre,
                    session.Descripcion,
                    usuariosSesiones
                };

                var result = await _dbContext.Database.ExecuteSqlRawAsync("spUpdateSession {0}, {1}, {2}, {3}, {4}", parameters);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<SesionesModel> GetTeacherSessionsById(int id, int teacherId)
        {
            try
            {
                var session = (await _dbContext.SesionesModel
                    .FromSqlRaw("EXECUTE spGetTeacherSessionById {0}, {1}", id, teacherId)
                    .ToListAsync())
                    .FirstOrDefault();

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<object> GetStudentSessionById(int id, int studentId)
        {
            try
            {
                var session = await _dbContext.Sesiones
                    .Select(sesion => new
                    {
                        sesion.Id,
                        sesion.Nombre,
                        sesion.Descripcion,
                        NombreCentroEducativo = sesion.CentroEducativo.Nombre,
                        UsuariosSesiones = sesion.UsuariosSesiones.Select(usuarioSesion => new { usuarioSesion.UsuarioId, usuarioSesion.SesionId, usuarioSesion.Eliminado }).Where(usuarioSesion => usuarioSesion.UsuarioId == studentId && usuarioSesion.SesionId == id).SingleOrDefault(),
                        sesion.FechaCreacion,
                        sesion.Eliminado
                    })
                .Where(sesion => sesion.Id == id && sesion.UsuariosSesiones.UsuarioId == studentId && sesion.Eliminado == false &&
                sesion.UsuariosSesiones.Eliminado == false)
                .SingleOrDefaultAsync();

                return session;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<object> GetSessionStudentActivity(int sessionId)
        {
            try
            {
                var studentActivity = await _dbContext.UsuarioRealizaPruebas.
                    Select(realizaPrueba => new { realizaPrueba.SessionId, realizaPrueba.IntentoCompletado, realizaPrueba.FechaCreacion, realizaPrueba.FechaCompletado, realizaPrueba.Calificacion, PruebaDiagnostica = realizaPrueba.PruebaDiagnostica, Usuario = realizaPrueba.Usuario })
                    .Where(realizaPrueba => realizaPrueba.SessionId == sessionId)
                    .ToListAsync();

                return studentActivity;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
