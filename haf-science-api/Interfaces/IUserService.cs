﻿using haf_science_api.Models;
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
        public Task<T> GetUsuarioById(int id);
        public Task Register(T user);
        public Task Update(T dataObject);
        public Task Delete(int id);
        public Task<IEnumerable<PaginatedUsuariosView>> GetPaginatedUsers(int page, int pageSize);
        public Task<int> GetPaginatedUsersCount();
        public Task<IEnumerable<PaginatedUsuariosView>> GetPaginatedUsersBy(int page, int pageSize, int? centroEducativoId,
            string username, string name, string correoElectronico, int? roldId);
        public Task<int> GetPaginatedUsersCountBy(int? centroEducativoId, string username, string name, string correoElectronico, int? rolId);
    }
}
