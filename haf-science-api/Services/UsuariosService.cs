using AutoMapper;
using haf_science_api.Interfaces;
using haf_science_api.Models;
using haf_science_api.Viewmodels;
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
                var user = await _dbContext.Usuarios.Where(x => x.NombreUsuario == username && x.Contrasena == password)
                .Include(x => x.Rol)
                .SingleOrDefaultAsync();

                var userDetails = await _dbContext.UsuariosDetalles.Where(x => x.Id == user.UsuarioDetalleId).FirstOrDefaultAsync();

                var userInfo = new UsuarioModel
                {
                    Id = user.Id,
                    Nombres = userDetails.Nombres,
                    Apellidos = userDetails.Apellidos,
                    NombreUsuario = user.NombreUsuario,
                    Contrasena = user.Contrasena,
                    CorreoElectronico = userDetails.CorreoElectronico,
                    Rol = user.Rol.Nombre
                };
                return userInfo;
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
                var usuario = await _dbContext.Usuarios.Where(x => x.NombreUsuario == username).FirstOrDefaultAsync();

                //var usuario = new UsuarioModel()
                //{
                //    Id = usuario.Id,

                //}

                return _mapper.Map<UsuarioModel>(usuario);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        public async Task<UsuarioModel> GetDataById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task SaveMultiple(IEnumerable<UsuarioModel> dataCollection)
        {
            throw new NotImplementedException();
        }

        public async Task SaveSingle(UsuarioModel dataObject)
        {
            throw new NotImplementedException();
        }

        public async Task Update(UsuarioModel dataObject)
        {
            throw new NotImplementedException();
        }
        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<UsuarioModel>> GetData()
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

                //UsuarioModel user = new UsuarioModel()
                //{
                //    Nombres = model.Nombres,
                //    Apellidos = model.Apellidos,
                //    CentroEducativoId = model.CentroEducativoId,
                //    FechaNacimiento = model.FechaNacimiento,
                //    Telefono = model.Telefono,
                //    CorreoElectronico = model.CorreoElectronico,
                //    NombreUsuario = model.NombreUsuario,
                //    Salt = stringSalt,
                //    Contrasena = hashedNewPassword,
                //    RolId = model.RolId,
                //    EstadoId = model.EstadoId
                //};

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
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IEnumerable<UsuarioModel>> GetUsers()
        {
            throw new NotImplementedException();
        }
    }
}
