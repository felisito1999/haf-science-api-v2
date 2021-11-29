using System;
using haf_science_api.Interfaces;
using System.Threading.Tasks;
using haf_science_api.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace haf_science_api.Services
{
    public class PreguntasService : IPreguntasService<Pregunta>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger _logger;

        public PreguntasService(HafScienceDbContext dbContext, ILogger<PreguntasService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Pregunta>> GetPruebasDiagnosticasPreguntas(int pruebasDiagnosticaId)
        {
            var pruebaDiagnostica = await _dbContext.PruebasDiagnosticas
                .Where(prueba => prueba.Id == pruebasDiagnosticaId)
                .Include(prueba => prueba.PruebasPregunta)
                .SingleOrDefaultAsync();

            var preguntas = _dbContext.Preguntas
                .Where(pregunta => pruebaDiagnostica.PruebasPregunta
                .Any(pruebaPregunta => pruebaDiagnostica.PruebasPregunta.Contains(pruebaPregunta)));
             
            return preguntas; 
        }

        public async Task<IEnumerable<object>> GetTeacherPreguntas(int teacherId)
        {
            try
            {
                var bancoPreguntas = await _dbContext.Preguntas
                .Where(x => x.CreadoPor == teacherId && x.Eliminado == false)
                .Select(pregunta => new { pregunta.Id, pregunta.Titulo, 
                Respuesta = pregunta.Respuesta.Select( respuesta => new { respuesta.Contenido, respuesta.EsCorrecta})
                }).ToListAsync();

                return bancoPreguntas;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetTeacherPreguntasByTitle(int teacherId, string title)
        {
            try
            {
                var preguntas = await _dbContext.Preguntas
                .Where(x => x.CreadoPor == teacherId && x.Titulo.Contains(title) && x.Eliminado == false)
                .Select(pregunta => new {
                    pregunta.Id,
                    pregunta.Titulo,
                    Respuesta = pregunta.Respuesta.Select(respuesta => new { respuesta.Contenido, respuesta.EsCorrecta })
                }).ToListAsync();

                return preguntas;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task SavePreguntas(Pregunta pregunta, int teacherId)
        {
            try
            {
                pregunta.CategoriaPreguntaId = 1;
                pregunta.EstadoId = 7;
                pregunta.CreadoPor = teacherId;

                foreach(var respuesta in pregunta.Respuesta)
                {
                    respuesta.CreadoPor = teacherId;
                    respuesta.EstadoId = 9;
                }
                await _dbContext.Preguntas.AddAsync(pregunta);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}
