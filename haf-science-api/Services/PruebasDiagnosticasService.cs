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
                var relationshipExists = await _dbContext.PruebasSesiones
                    .Where(pruebaSesion => pruebaSesion.PruebaDiagnosticaId == model.PruebaDiagnosticaId && 
                    pruebaSesion.SesionId == model.SessionId).AnyAsync();

                if (relationshipExists)
                {
                    throw new Exception("Esta prueba ya se encuentra relacionada con esta sesión");
                }

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
                    DuracionMinutos = model.DuracionMinutos,
                    CantidadIntentos = model.CantidadIntentos,
                    FechaInicio = model.FechaInicio.ToLocalTime(),
                    FechaLimite = model.FechaLimite.ToLocalTime(),
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
                    .Select(prueba => new { prueba.Id, prueba.Titulo,
                        PruebaSesion = prueba.PruebasSesiones.Select(pruebaSesion => new { pruebaSesion.SesionId, pruebaSesion.FechaInicio, pruebaSesion.FechaLimite, pruebaSesion.DuracionMinutos, pruebaSesion.Eliminado }).Where(pruebaSesion => pruebaSesion.SesionId == sessionId).SingleOrDefault(),
                        prueba.Eliminado })
                    .Where(prueba => prueba.Eliminado == false && 
                        prueba.PruebaSesion.SesionId == sessionId && 
                        prueba.PruebaSesion.Eliminado == false)
                    .OrderBy(prueba => prueba.PruebaSesion.FechaInicio)
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
                    .Select(prueba => new {
                        prueba.Id,
                        prueba.Titulo,
                        PruebaSesion = prueba.PruebasSesiones
                        .Select(pruebaSesion => new { pruebaSesion.SesionId, pruebaSesion.FechaInicio, pruebaSesion.FechaLimite, pruebaSesion.DuracionMinutos, pruebaSesion.Eliminado }).Where( pruebaSesion => pruebaSesion.SesionId == sessionId).SingleOrDefault(),
                        prueba.Eliminado
                    })
                    .Where(prueba => prueba.Eliminado == false &&
                        prueba.PruebaSesion.SesionId == sessionId &&
                        prueba.PruebaSesion.Eliminado == false)
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

                double valoracion = !pruebaDiagnostica.Preguntas.Any() && 
                    pruebaDiagnostica.PruebaDiagnostica.CalificacionMaxima != 0 ?
                    0.00 : 
                    (double)pruebaDiagnostica.PruebaDiagnostica.CalificacionMaxima / pruebaDiagnostica.Preguntas.Count();
                
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
        public async Task<object> IsAvailableForStudent(int studentId, int sessionId, int pruebaDiagnosticaId)
        {
            try
            {
                var pruebaInfo = await _dbContext.PruebasDiagnosticas
                    .Select(pruebaDiagnostica => new {
                        pruebaDiagnostica.Id,
                        pruebaDiagnostica.Titulo,
                        PruebasSesiones = pruebaDiagnostica.PruebasSesiones
                        .Select(pruebaSesion => new { pruebaSesion.DuracionMinutos, pruebaSesion.SesionId, pruebaSesion.FechaInicio, pruebaSesion.FechaLimite, pruebaSesion.Eliminado})
                        .Where(pruebaSesion => pruebaSesion.SesionId == sessionId && pruebaSesion.Eliminado == false).FirstOrDefault(),
                        Preguntas = pruebaDiagnostica.PruebasPregunta.Where(pruebaPregunta => pruebaPregunta.PruebaId == pruebaDiagnosticaId).Select(pruebaPregunta => new { pruebaPregunta.PreguntaId, pruebaPregunta.TituloPregunta, Respuestas = pruebaPregunta.Pregunta.Respuesta.Select(respuesta => new { respuesta.Id, respuesta.Contenido, selected = false })}),
                        pruebaDiagnostica.Eliminado
                    })
                    .Where(pruebaDiagnostica => pruebaDiagnostica.Id == pruebaDiagnosticaId && 
                    pruebaDiagnostica.PruebasSesiones.SesionId == sessionId && 
                    pruebaDiagnostica.Eliminado == false)
                    .SingleOrDefaultAsync();

                var usuarioRealizaPrueba = await _dbContext.UsuarioRealizaPruebas
                    .Where(realizaPrueba => realizaPrueba.SessionId == sessionId &&
                    realizaPrueba.PruebaDiagnosticaId == pruebaDiagnosticaId &&
                    realizaPrueba.UsuarioId == studentId && 
                    realizaPrueba.IntentoCompletado == true)
                    .CountAsync();


                if (!(pruebaInfo.PruebasSesiones.FechaInicio <= DateTime.Now &&
                    pruebaInfo.PruebasSesiones.FechaLimite >= DateTime.Now))
                {
                    throw new Exception("No puede tomar la prueba porque esta fuera del tiempo especificado");
                }

                //Validación temporal en lo que se agrega la parte de los intentos.
                if (usuarioRealizaPrueba > 0)
                {
                    throw new Exception("Ya ha realizado esta prueba en esta sesión");
                }

                if (pruebaInfo == null)
                {
                    throw new Exception("La prueba solicitada no es válida");
                }

                var studentSessions = await _dbContext.UsuariosSesiones
                    .Where(usuarioSesion => usuarioSesion.UsuarioId == studentId &&
                    usuarioSesion.Eliminado == false && usuarioSesion.SesionId == sessionId)
                    .ToListAsync();
                
                if(!studentSessions.Any())
                {
                    throw new Exception("La prueba solicitada no es válida");
                }
                
                //foreach (var studentSession in pruebaInfo.PruebasSesiones)
                //{
                //    bool isAvailable = pruebaInfo.PruebasSesiones.Where(pruebaSesion => pruebaSesion.SesionId == studentSession.SesionId).Any();

                //    if (isAvailable)
                //    {
                //        return pruebaInfo;
                //    }
                //}

                return pruebaInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
        public async Task EvaluateTestGrade(AttemptModel attempt, int sessionId, int studentId)
        {
            try
            {
                var pruebaPreguntas = await _dbContext.PruebasDiagnosticas
                    .Select(prueba => new { prueba.Id, Preguntas =  prueba.PruebasPregunta.Select(pruebaPregunta => new { pruebaPregunta.Pregunta.Id, pruebaPregunta.Valoracion, Respuestas = pruebaPregunta.Pregunta.Respuesta.Select(respuesta => new { respuesta.Id, respuesta.EsCorrecta})})}) 
                    .Where(prueba => prueba.Id == attempt.PruebaId)
                    .SingleOrDefaultAsync();

                //var pruebaPreguntas = await _dbContext.PruebasDiagnosticas
                //    .Select(prueba => new { prueba.Id, PruebasPreguntas = new { pruebaPregunta.Pregunta.Id, Respuestas = pruebaPregunta.Pregunta.Respuesta.Select(respuesta => new { respuesta.Id, respuesta.EsCorrecta }) } } )
                //    .Where(pruebaPregunta => pruebaPregunta.PruebaId == attempt.PruebaId)
                //    .ToListAsync();
                if (pruebaPreguntas == null)
                {
                    throw new Exception("Ha ocurrido un error");
                }

                double calificacion = 0;

                foreach(var pregunta in pruebaPreguntas.Preguntas)
                {
                    foreach (var respuesta in pregunta.Respuestas)
                    {
                        var attemptPreguntaInfo = attempt.Preguntas
                            .Where(attemptPregunta => attemptPregunta.PreguntaId == pregunta.Id)
                            .SingleOrDefault();

                        var selectedRespuesta = attemptPreguntaInfo.Respuestas
                            .Where(attemptRespuesta => attemptRespuesta.Id == respuesta.Id)
                            .SingleOrDefault();

                        if (selectedRespuesta.Selected && respuesta.EsCorrecta)
                        {
                            calificacion += (double)pregunta.Valoracion;
                        }
                        //foreach(var attemptRespuesta in attemptPreguntaInfo.Respuestas)
                        //{
                        //    if (attemptRespuesta.)
                        //}
                    }
                }
                var usuarioRealizaPrueba = await _dbContext.UsuarioRealizaPruebas
                    .Where(realizaPrueba => realizaPrueba.PruebaDiagnosticaId == attempt.PruebaId && 
                    realizaPrueba.UsuarioId == studentId && realizaPrueba.SessionId == attempt.SessionId)
                    .SingleOrDefaultAsync();

                if (usuarioRealizaPrueba != null)
                {
                    usuarioRealizaPrueba.IntentoCompletado = true;
                    usuarioRealizaPrueba.FechaCompletado = DateTime.Now;
                    usuarioRealizaPrueba.Calificacion = (decimal)Math.Ceiling(calificacion);

                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Ha ocurrido un error al momento de guardar el intento de prueba");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task StartTest(int pruebaDiagnosticaId, UsuariosModel student, int sessionId)
        {
            try
            {
                var usuarioRealizaPruebas = await _dbContext.UsuarioRealizaPruebas
                    .Where(realizacion => realizacion.PruebaDiagnosticaId == pruebaDiagnosticaId &&
                    realizacion.SessionId == sessionId && realizacion.UsuarioId == student.Id)
                    .SingleOrDefaultAsync();

                if (usuarioRealizaPruebas == null)
                {
                    var pruebaDiagnostica = await GetPruebaDiagnosticaById(pruebaDiagnosticaId);

                    if (pruebaDiagnostica == null)
                    {
                        throw new Exception("La prueba diagnóstica no es válida");
                    }

                    UsuarioRealizaPrueba usuarioRealizaPrueba = new UsuarioRealizaPrueba()
                    {
                        UsuarioId = student.Id, 
                        PruebaDiagnosticaId = pruebaDiagnosticaId,
                        SessionId = sessionId,
                        NombreUsuario = student.NombreUsuario,
                        TituloPrueba = pruebaDiagnostica.Titulo,
                        Calificacion = 0,
                        FechaCompletado = null,
                        IntentoCompletado = false,
                        FechaCreacion = DateTime.Now
                    };

                    await _dbContext.UsuarioRealizaPruebas
                        .AddAsync(usuarioRealizaPrueba);

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<PruebasDiagnostica> GetPruebaDiagnosticaById(int pruebaDiagnosticaId)
        {
            try
            {
                var pruebaDiagnostica = await _dbContext.PruebasDiagnosticas
                    .Where(prueba => prueba.Id == pruebaDiagnosticaId && prueba.Eliminado == false)
                    .SingleOrDefaultAsync();

                return pruebaDiagnostica;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<object> GetSessionPruebaDiagnosticaById(int pruebaDiagnosticaId, int sessionId)
        {
            try
            {
                var pruebaDiagnostica = await _dbContext.PruebasDiagnosticas.
                    Select(prueba => new
                    {
                        prueba.Id,
                        prueba.Titulo,
                        prueba.CalificacionMaxima,
                        PruebaSesion = prueba.PruebasSesiones.Select(pruebaSesion => new { pruebaSesion.SesionId, Duracion = pruebaSesion.DuracionMinutos, pruebaSesion.FechaInicio, pruebaSesion.FechaLimite, pruebaSesion.Eliminado }).Where(pruebaSesion => pruebaSesion.SesionId == sessionId).SingleOrDefault(),
                        CantidadPreguntas = prueba.PruebasPregunta.Count(),
                        prueba.Eliminado
                    })
                    .Where(prueba => prueba.Id == pruebaDiagnosticaId && prueba.PruebaSesion.SesionId == sessionId)
                    .SingleOrDefaultAsync();

                return pruebaDiagnostica;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetPaginatedStudentPruebasDiagnosticasBySessionId(int studentId, int sessionId, int page, int pageSize)
        {
            try
            {
                //Falta agregar la parte de que se presenten las sesiones que pertenecen a una sesión.
                int skip = (page - 1) * pageSize;

                var pruebasDiagnosticas = await _dbContext.PruebasDiagnosticas
                    .Select(prueba => new {
                        prueba.Id,
                        prueba.Titulo,
                        prueba.CalificacionMaxima,
                        PruebaSesion = prueba.PruebasSesiones.Select(pruebaSesion => new { pruebaSesion.SesionId, pruebaSesion.FechaInicio, pruebaSesion.FechaLimite, pruebaSesion.DuracionMinutos, pruebaSesion.Eliminado }).Where(pruebaSesion => pruebaSesion.SesionId == sessionId).SingleOrDefault(),
                        UsuarioRealizaPrueba = prueba.UsuarioRealizaPruebas.Select(realizaPrueba => new { realizaPrueba.UsuarioId, realizaPrueba.PruebaDiagnosticaId, realizaPrueba.SessionId, realizaPrueba.Calificacion, realizaPrueba.IntentoCompletado, Porcentaje = realizaPrueba.IntentoCompletado == true ? realizaPrueba.Calificacion > 0 ? (prueba.CalificacionMaxima / realizaPrueba.Calificacion) * 100 : 0 : 0 }).Where(realizaPrueba => realizaPrueba.UsuarioId == studentId && realizaPrueba.SessionId == sessionId).SingleOrDefault(),
                        prueba.Eliminado
                    })
                    .Where(prueba => 
                        prueba.Eliminado == false &&
                        prueba.PruebaSesion.SesionId == sessionId &&
                        prueba.PruebaSesion.Eliminado == false)
                    .OrderBy(prueba => prueba.UsuarioRealizaPrueba.IntentoCompletado).ThenBy(prueba => prueba.PruebaSesion.FechaInicio)
                    .Skip(skip).Take(pageSize).ToListAsync();

                return pruebasDiagnosticas;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<int> GetPaginatedStudentPruebasDiagnosticasBySessionIdCount(int sessionId)
        {
            var pruebasDiagnosticasCount = await _dbContext.PruebasDiagnosticas
                    .Select(prueba => new
                    {
                        prueba.Id,
                        prueba.Titulo,
                        PruebaSesion = prueba.PruebasSesiones.Select(pruebaSesion => new { pruebaSesion.SesionId, pruebaSesion.FechaInicio, pruebaSesion.FechaLimite, pruebaSesion.DuracionMinutos, pruebaSesion.Eliminado }).Where(pruebaSesion => pruebaSesion.SesionId == sessionId).SingleOrDefault(),
                        UsuarioRealizaPrueba = prueba.UsuarioRealizaPruebas.Select(realizaPrueba => new { realizaPrueba.PruebaDiagnosticaId, realizaPrueba.SessionId, realizaPrueba.Calificacion, realizaPrueba.IntentoCompletado}).Where(realizaPrueba => realizaPrueba.SessionId == sessionId).SingleOrDefault(),
                        prueba.Eliminado
                    })
                    .Where(prueba => prueba.Eliminado == false &&
                        prueba.PruebaSesion.SesionId == sessionId &&
                        prueba.PruebaSesion.Eliminado == false)
                    .CountAsync();

            return pruebasDiagnosticasCount;
        }

        public async Task<object> GetSessionAverageGrades(int sessionId, int teacherId)
        {
            try
            {
                var grades = await _dbContext.UsuarioRealizaPruebas
                .Select(realizaPrueba => new { realizaPrueba.SessionId, realizaPrueba.PruebaDiagnostica.CalificacionMaxima, realizaPrueba.Calificacion,
                    Porcentaje = realizaPrueba.IntentoCompletado == true ? realizaPrueba.Calificacion > 0 ? ( realizaPrueba.Calificacion / realizaPrueba.PruebaDiagnostica.CalificacionMaxima ) * 100 : 0 : 0,
                    realizaPrueba.IntentoCompletado, 
                    SesionCreadoPor = realizaPrueba.Session.CreadoPor })
                .Where(intento => intento.SessionId == sessionId && intento.IntentoCompletado == true && intento.SesionCreadoPor == teacherId)
                .ToListAsync();

                if(grades.Any())
                {
                    double average = 0.00;

                    foreach (var grade in grades)
                    {
                        average += (double)grade.Porcentaje;
                    }

                    average /= grades.Count();

                    return new
                    {
                        NotaPromedio = average
                    };
                }

                return null; 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<object> GetSessionTestAverageGrades(int sessionId, int testId, int teacherId)
        {
            try
            {
                var grades = await _dbContext.UsuarioRealizaPruebas
                .Select(realizaPrueba => new {
                    realizaPrueba.PruebaDiagnosticaId,
                    realizaPrueba.SessionId,
                    realizaPrueba.PruebaDiagnostica.CalificacionMaxima,
                    realizaPrueba.Calificacion,
                    Porcentaje = realizaPrueba.IntentoCompletado == true ? realizaPrueba.Calificacion > 0 ? (realizaPrueba.Calificacion / realizaPrueba.PruebaDiagnostica.CalificacionMaxima) * 100 : 0 : 0,
                    realizaPrueba.IntentoCompletado,
                    SesionCreadoPor = realizaPrueba.Session.CreadoPor
                })
                .Where(intento => intento.SessionId == sessionId && intento.IntentoCompletado == true && intento.PruebaDiagnosticaId == testId && intento.SesionCreadoPor == teacherId)
                .ToListAsync();

                if(grades.Any())
                {
                    double average = 0.00;
                    
                    foreach(var grade in grades)
                    {
                        average += (double)grade.Porcentaje;
                    }

                    average /= grades.Count();

                    return new
                    {
                        NotaPromedio = average
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<object> GetStudentSessionAverageGrades(int sessionId, int studentId, int teacherId)
        {
            try
            {
                var grades = await _dbContext.UsuarioRealizaPruebas
                .Select(realizaPrueba => new {
                    realizaPrueba.PruebaDiagnosticaId,
                    realizaPrueba.UsuarioId,
                    realizaPrueba.SessionId,
                    realizaPrueba.PruebaDiagnostica.CalificacionMaxima,
                    realizaPrueba.Calificacion,
                    Porcentaje = realizaPrueba.IntentoCompletado == true ? realizaPrueba.Calificacion > 0 ? (realizaPrueba.Calificacion / realizaPrueba.PruebaDiagnostica.CalificacionMaxima ) * 100 : 0 : 0,
                    realizaPrueba.IntentoCompletado,
                    SesionCreadoPor = realizaPrueba.Session.CreadoPor
                })
                .Where(intento => intento.SessionId == sessionId && intento.IntentoCompletado == true && intento.UsuarioId == studentId && intento.SesionCreadoPor == teacherId)
                .ToListAsync();

                if (grades.Any())
                {
                    double average = 0.00;

                    foreach (var grade in grades)
                    {
                        average += (double)grade.Porcentaje;
                    }

                    average /= grades.Count();

                    return new
                    {
                        NotaPromedio = average
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<object> GetTestSessionGrades(int testId, int sessionId, int teacherId)
        {
            try
            {
                var pruebaInfo = await _dbContext.PruebasDiagnosticas.Select(prueba => new {prueba.Id, prueba.Titulo, prueba.CalificacionMaxima})
                    .Where(prueba => prueba.Id == testId).SingleOrDefaultAsync();

                var grades = await _dbContext.UsuariosSesiones
                    .Select(gradeRow => new { gradeRow.SesionId, 
                        Usuario = new { gradeRow.Usuario.Id, gradeRow.Usuario.NombreUsuario, PersonalInfo = gradeRow.Usuario.UsuarioDetalle }, 
                        RealizaPrueba  = gradeRow.Usuario.UsuarioRealizaPruebas.Where(attempt => attempt.PruebaDiagnosticaId == testId && attempt.SessionId == sessionId).SingleOrDefault(), gradeRow.Eliminado  })
                    .Where(gradeRow => gradeRow.SesionId == sessionId && gradeRow.Eliminado == false) 
                    .OrderByDescending(gradeRow => gradeRow.RealizaPrueba.IntentoCompletado)
                    .ToListAsync();

                if (grades.Any())
                {
                    return new {
                        PruebaInfo = pruebaInfo,
                        Grades = grades
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
