using haf_science_api.Models;
using haf_science_api.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Interfaces
{
    public interface IUserService<T>
        where T : class
    {
        public Task<T> GetUsuarioLoginInfo(string username, string password);
        public Task<T> GetUsuarioByUsername(string username);
        public Task<T> GetDataById(int id);
        public Task Register(T user);
        public Task Update(T dataObject);
        public Task Delete(int id);
        public Task<IEnumerable<T>> GetUsers();
    }
}
