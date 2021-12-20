using System;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;

namespace haf_science_api.Services
{
    public class InsigniasService : IInsigniasService<Insignia>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly ILogger<InsigniasService> _logger;

        public InsigniasService(HafScienceDbContext dbContext, ILogger<InsigniasService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

        }
        public async Task AssignInsigniaToSessionStudent(UsuariosSesionesInsignia usuariosSesionesInsignia)
        {
            try
            { 
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    var userBadges = _dbContext.UsuariosSesionesInsignias.Where(insignia => insignia.SesionId == usuariosSesionesInsignia.SesionId && 
                    insignia.UsuarioId == usuariosSesionesInsignia.UsuarioId);
                
                    if (userBadges.Any())
                    {
                        foreach (var userBadge in userBadges)
                        {
                            if (userBadge.InsigniaId == usuariosSesionesInsignia.InsigniaId)
                            {
                                throw new Exception("Este usuario ya tiene esta insignia asignada");
                            }
                            userBadge.IsFavorite = false;
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                    usuariosSesionesInsignia.IsFavorite = true;
                    await _dbContext.UsuariosSesionesInsignias.AddAsync(usuariosSesionesInsignia);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<object> GetImagenesInsignias()
        {
            try
            {
                var imagenesInsignias = await _dbContext.ImagenesInsignias.ToListAsync();

                return imagenesInsignias;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<Insignia> GetInsigniaById(int insigniaId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetInsigniaCreatedBy(int teacherId)
        {
            try
            {
                var insignias = await _dbContext.Insignias
                    .Select(insignia => new { insignia.Id, insignia.Nombre, insignia.Descripcion, Imagen = insignia.ImagenesInsigniasNavigation.ContenidoSvg, insignia.CreadoPor, insignia.Eliminado })
                    .Where(insignia => insignia.CreadoPor == teacherId && insignia.Eliminado == false)
                    .ToListAsync();

                return insignias;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetStudentSessionsInsignias(int studentId, int sessionId)
        {
            try
            {
                var insignias = await _dbContext.UsuariosSesionesInsignias
                    .Select(insignia => new { insignia.SesionId, insignia.UsuarioId, Insignia = new { insignia.Insignia.Nombre, insignia.Insignia.Descripcion, insignia.Insignia.ImagenesInsigniasNavigation.ContenidoSvg } })
                    .Where(insignia => insignia.SesionId == sessionId && insignia.UsuarioId == studentId)
                    .ToListAsync();

                return insignias; 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<object> GetFavoriteSessionUserInsignia(int studentId, int sessionId)
        {
            try
            {
                var insignia = await  _dbContext.UsuariosSesionesInsignias.Select(insignia => new { insignia.InsigniaId, insignia.SesionId, insignia.UsuarioId, Imagen = insignia.Insignia.ImagenesInsigniasNavigation.ContenidoSvg })
                    .Where(insignia => insignia.UsuarioId == studentId && insignia.SesionId == sessionId)
                    .FirstOrDefaultAsync();

                return insignia;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task SaveBadge(Insignia insignia)
        {
            try
            {
                await _dbContext.Insignias.AddAsync(insignia);
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
