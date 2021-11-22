using haf_science_api.Models;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IUserHashesService<T> where T : class
    {
        Task<T> GetUserHashByHash(string hash);
    }
}
