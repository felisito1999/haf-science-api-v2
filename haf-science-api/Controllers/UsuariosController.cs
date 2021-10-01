using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUserService<UsuarioModel> _userService;
        public UsuariosController(IUserService<UsuarioModel> userService)
        {
            _userService = userService;
        }
        //[HttpGet]
        //[Authorize(Roles = "Administrador")]
        //public async Task<ActionResult> Get([FromQuery] int page, [FromQuery]int pageSize)
        //{
        //    try
        //    {
        //        var users = await _userService.GetPaginatedUsers(page, pageSize);
        //        var totalRecords = await _userService.GetPaginatedUsersCount();

        //        return StatusCode(StatusCodes.Status200OK,
        //            new PaginatedResponse<UsuarioView> { Records = users, RecordsTotal = totalRecords});
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        [Route("{id}")]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var users = await _userService.GetUsuarioById(id);

                return Ok(users);
            }
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> GetPaginated([FromQuery]int page, [FromQuery]int pageSize, int? id, int? centroEducativoId, string username, string name, string correoElectronico, int? rolId)
        {
            //Faltan los query por roles y centros educativos;
            if (id.HasValue)
            {
                var user = await _userService.GetUsuarioById((int)id);

                return Ok(user);
            }
            if(!string.IsNullOrWhiteSpace(name) | centroEducativoId.HasValue | !string.IsNullOrWhiteSpace(username) | !string.IsNullOrWhiteSpace(correoElectronico) | rolId.HasValue)
            {
                var users = await _userService.GetPaginatedUsersBy(page, pageSize, centroEducativoId, username, name, correoElectronico, rolId);
                var totalRecords = await _userService.GetPaginatedUsersCountBy(centroEducativoId, username, name, correoElectronico, rolId);

                return StatusCode(StatusCodes.Status200OK,
                    new PaginatedResponse<UsuarioView> { Records = users, RecordsTotal = totalRecords });
            }
            else
            {
                var users = await _userService.GetPaginatedUsers(page, pageSize);
                var totalRecords = await _userService.GetPaginatedUsersCount();

                return StatusCode(StatusCodes.Status200OK,
                    new PaginatedResponse<UsuarioView> { Records = users, RecordsTotal = totalRecords });
            }
        }

    }
}
