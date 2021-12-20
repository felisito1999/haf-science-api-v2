using haf_science_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IInsigniasService<T> where T : class
    {
        Task<Insignia> GetInsigniaById (int insigniaId);
        Task<IEnumerable<object>> GetInsigniaCreatedBy(int teacherId);
        Task SaveBadge (Insignia insignia);
        Task<IEnumerable<object>> GetStudentSessionsInsignias (int studentId, int sessionId);
        Task AssignInsigniaToSessionStudent (UsuariosSesionesInsignia usuariosSesionesInsignia);
        Task<object> GetFavoriteSessionUserInsignia(int studentId, int sessionId);
        Task<object> GetImagenesInsignias();
    }
}
