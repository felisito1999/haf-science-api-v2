using haf_science_api.Models;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IInsigniasService<T> where T : class
    {
        Task<Insignia> GetInsigniaById (int insigniaId);
        Task SaveBadge (Insignia insignia);
        Task<object> GetStudentSessionsInsignias (int studentId, int sessionId);
        Task AssignInsigniaToSessionStudent (int insigniaId, int studentId, int sessionId);
    }
}
