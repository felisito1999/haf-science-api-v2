using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Services
{
    public class UsuariosService : IUserService<UsuarioModel>
    {
        private readonly HafScienceDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;
        //private readonly ILogger _logger;
        public UsuariosService(HafScienceDbContext dbContext, IPasswordService passwordService, IMapper mapper) //ILogger logger)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _mapper = mapper;
            //_logger = logger; 
        }
        public async Task<UsuarioModel> GetUsuarioLoginInfo(string username, string password)
        {
            try
            {
                var user = await GetUsuarioByUsername(username);

                if (user != null)
                {
                    var byteSalt = _passwordService.ConvertStringSaltToByteA(user.Salt);
                    var hashedPassword = _passwordService.HashPassword(password, byteSalt);

                    if (user.Contrasena != hashedPassword)
                    {
                        user = null;
                        return user;
                    }
                    //user = (await _dbContext.UsuariosModel.FromSqlRaw("EXECUTE spGetUserByUsernamePassword {0}, {1}", username, enteredPassword).ToListAsync()).FirstOrDefault();
                }
                //var user = await _dbContext.Usuarios.Where(x => x.NombreUsuario == username && x.Contrasena == password)
                //.Include(x => x.Rol)
                //.SingleOrDefaultAsync();

                //var userDetails = await _dbContext.UsuariosDetalles.Where(x => x.Id == user.UsuarioDetalleId).FirstOrDefaultAsync();

                //var userInfo = new UsuarioModel
                //{
                //    Id = user.Id,
                //    Nombres = userDetails.Nombres,
                //    Apellidos = userDetails.Apellidos,
                //    NombreUsuario = user.NombreUsuario,
                //    Contrasena = user.Contrasena,
                //    CorreoElectronico = userDetails.CorreoElectronico,
                //    NombreRol = user.Rol.Nombre
                //};
                return user;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        public async Task<UsuarioModel> GetUsuarioByUsername(string username)
        {
            try
            {
                var usuario = (await _dbContext.UsuariosModel.FromSqlRaw("EXECUTE spGetUserDataByUsername {0}", username).ToListAsync()).FirstOrDefault();

                return usuario;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        public async Task Update(UsuarioModel dataObject)
        {
            throw new NotImplementedException();
        }
        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Register(UsuarioModel user)
        {
            try
            {
                byte[] salt = _passwordService.GetSalt();
                string stringSalt = _passwordService.ConvertSaltToString(salt);
                string newPassword = _passwordService.CreateDefaultUserPassword(user.Nombres, user.Apellidos, user.FechaNacimiento);
                string hashedNewPassword = _passwordService.HashPassword(newPassword, salt);

                var parameters = new object[]
                {
                user.Nombres,
                user.Apellidos,
                user.FechaNacimiento,
                user.Telefono,
                user.CorreoElectronico,
                user.NombreUsuario,
                stringSalt,
                hashedNewPassword,
                user.RolId,
                user.EstadoId,
                user.CentroEducativoId,
                user.CreadoPor
                };

                await _dbContext.Database.ExecuteSqlRawAsync("spRegisterUser {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}",
                    parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<UsuarioModel>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<UsuarioModel> GetUsuarioById(int id)
        {
            var user = (await _dbContext.UsuariosModel.FromSqlRaw("EXECUTE spGetUserDataById {0}", id).ToListAsync()).FirstOrDefault();

            return user;
        }
    }
}
