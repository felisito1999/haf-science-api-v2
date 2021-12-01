using System;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace haf_science_api.Services
{
    public class PruebasDiagnosticasService : IPruebasDiagnosticasService<PruebasDiagnostica>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;

        public PruebasDiagnosticasService(HafScienceDbContext context, ILogger<PruebasDiagnosticasService> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task AssignToSession(AssignTestToSessionModel model, int teacherId)
        {
            try
            {
                var prueba = await _dbContext.PruebasDiagnosticas
                    .Where(prueba => prueba.Id == model.PruebaDiagnosticaId)
                    .FirstOrDefaultAsync();

                var session = await _dbContext.Sesiones
                    .Where(session => session.Id == model.SessionId)
                    .FirstOrDefaultAsync();

                var pruebaSesion = new PruebasSesione()
                {
                    PruebaDiagnosticaId = model.PruebaDiagnosticaId,
                    SesionId = model.SessionId,
                    TituloPrueba = prueba.Titulo,
                    NombreSesion = session.Nombre,
                    FechaInicio = model.FechaInicio,
                    FechaLimite = model.FechaLimite,
                };
                await _dbContext.PruebasSesiones.AddAsync(pruebaSesion);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetPaginatedPruebasDiagnosticasBySessionId(int sessionId, int page, int pageSize)
        {
            try
            {
                //Falta agregar la parte de que se presenten las sesiones que pertenecen a una sesión.
                int skip = (page - 1) * pageSize;
                var pruebasDiagnosticas = await _dbContext.PruebasDiagnosticas
                    .Select(prueba => new { prueba.Id, prueba.Titulo, SesionId = prueba.PruebasSesiones.Select(sesion => sesion.SesionId).SingleOrDefault(), prueba.Eliminado})
                    .Where(prueba => prueba.Eliminado == false && prueba.SesionId == sessionId)
                    .Skip(skip).Take(pageSize).ToListAsync();

                return pruebasDiagnosticas;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedPruebasDiagnosticasBySessionIdCount(int sessionId)
        {
            try
            {
                int pruebaDiagnosticasCount = await _dbContext.PruebasDiagnosticas
                    .Where(x => x.Eliminado == false)
                    .CountAsync();

                return pruebaDiagnosticasCount;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<PruebasDiagnostica>> GetTeacherPaginatedPruebasDiagnosticas(int teacherId, int page, int pageSize)
        {
            try
            {
                int skip = (page - 1) * pageSize;
                var pruebasDiagnosticas = await _dbContext.PruebasDiagnosticas
                    .Where(x => x.CreadoPor == teacherId && x.Eliminado == false)
                    .Skip(skip).Take(pageSize).ToListAsync();

                return pruebasDiagnosticas;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetTeacherPaginatedPruebasDiagnosticasCount(int teacherId)
        {
            try
            {
                var pruebasCount = await _dbContext.PruebasDiagnosticas
                    .Where(x => x.CreadoPor == teacherId && x.Eliminado == false)
                    .CountAsync();

                return pruebasCount;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task SavePruebaDiagnostica(PruebasDiagnosticasModel pruebaDiagnostica, int teacherId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
               
                var pruebaDiagnosticaId = await _dbContext.PruebasDiagnosticas.AddAsync(pruebaDiagnostica.PruebaDiagnostica);
                await _dbContext.SaveChangesAsync();

                List<PruebasPregunta> pruebasPreguntas = new List<PruebasPregunta>();

                double valoracion = pruebaDiagnostica.Preguntas.Any() ? 0.00 : (double)pruebaDiagnostica.PruebaDiagnostica.CalificacionMaxima / pruebaDiagnostica.Preguntas.Count();
                
                foreach(var pregunta in pruebaDiagnostica.Preguntas)
                {
                    PruebasPregunta pruebaPregunta = new PruebasPregunta()
                    {
                        PreguntaId = pregunta.Id,
                        PruebaId = pruebaDiagnostica.PruebaDiagnostica.Id,
                        TituloPrueba = pruebaDiagnostica.PruebaDiagnostica.Titulo,
                        TituloPregunta = pregunta.Titulo,
                        CalificacionMaximaPrueba = pruebaDiagnostica.PruebaDiagnostica.CalificacionMaxima,
                        Valoracion = (decimal)valoracion
                    };

                    pruebasPreguntas.Add(pruebaPregunta);
                }
                await _dbContext.PruebasPreguntas.AddRangeAsync(pruebasPreguntas);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
