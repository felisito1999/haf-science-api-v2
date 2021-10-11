﻿using haf_science_api.Interfaces;
using haf_science_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IUserService<UsuariosModel> _userService;
        private readonly ILogger _logger;
        public UsuariosController(IUserService<UsuariosModel> userService, ILogger<UsuariosController> logger)
        {
            _userService = userService;
            _logger = logger; 
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Get([FromQuery]int page, [FromQuery]int pageSize, int? id, int? centroEducativoId, string username, string name, string correoElectronico, int? rolId)
        {
            try
            {
                if (id.HasValue)
                {
                    var user = await _userService.GetUsuarioById((int)id);

                    if (user == null)
                    {
                        return NotFound();
                    }

                    return Ok(user);
                }
                if (!string.IsNullOrWhiteSpace(name) | centroEducativoId.HasValue | !string.IsNullOrWhiteSpace(username) | !string.IsNullOrWhiteSpace(correoElectronico) | rolId.HasValue)
                {
                    var users = await _userService.GetPaginatedUsersBy(page, pageSize, centroEducativoId, username, name, correoElectronico, rolId);
                    var totalRecords = await _userService.GetPaginatedUsersCountBy(centroEducativoId, username, name, correoElectronico, rolId);

                    return StatusCode(StatusCodes.Status200OK,
                        new PaginatedResponse<PaginatedUsuariosView> { Records = users, RecordsTotal = totalRecords });
                }
                else
                {
                    var users = await _userService.GetPaginatedUsers(page, pageSize);
                    var totalRecords = await _userService.GetPaginatedUsersCount();

                    return StatusCode(StatusCodes.Status200OK,
                        new PaginatedResponse<PaginatedUsuariosView> { Records = users, RecordsTotal = totalRecords });
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        [HttpPut]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Update([FromBody]UsuariosModel usuario)
        {
            if (usuario != null)
            {
                await _userService.Update(usuario);

                return Ok();
            }

            return BadRequest();
        }
        [HttpDelete]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userService.Delete(id);

                return Ok(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
                throw;
            }
        }

    }
}
