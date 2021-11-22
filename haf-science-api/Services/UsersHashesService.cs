using System;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace haf_science_api.Services
{
    public class UsersHashesService : IUserHashesService<UserHash>
    {
        private readonly HafScienceDbContext _dbContext;

        public UsersHashesService(HafScienceDbContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public async Task<UserHash> GetUserHashByHash(string hash)
        {
            try
            {
                var userHash = await _dbContext.UserHashes
                    .Where(x => x.Hash == hash && x.FechaExpiracion >= DateTime.UtcNow)
                    .SingleOrDefaultAsync();
                
                return userHash;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }
    }
}
