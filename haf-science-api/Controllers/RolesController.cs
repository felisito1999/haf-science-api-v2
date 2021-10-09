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
    public class RolesController : ControllerBase
    {
        private readonly IDataService<Role, RolView> _rolesService;
        public RolesController(IDataService<Role, RolView> rolesService)
        {
            _rolesService = rolesService;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var roles = await _rolesService.GetAll();

                return Ok(roles);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new Response { 
                        Status = "Error",
                        Message = "Ha ocurrido un error con su solicitud"
                    });
                throw;
            }
        }
    }
}
