using haf_science_api.Interfaces;
using haf_science_api.Models;
using haf_science_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace haf_science_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PreguntasController : ControllerBase
    {
        private readonly IPreguntasService<Pregunta> _preguntasService; 

        public PreguntasController(IPreguntasService<Pregunta> preguntasService)
        {
            _preguntasService = preguntasService; 
        }
        [Route("banco-preguntas")]
        [HttpGet]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> GetBancoPreguntas()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

            var preguntas = await _preguntasService.GetTeacherPreguntas(teacherId);

            return Ok(preguntas);
        }
        [Route("banco-preguntas-by-title")]
        [HttpGet]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> GetPreguntasByTitle(string title)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

            var preguntas = await _preguntasService.GetTeacherPreguntasByTitle(teacherId, title);

            return Ok(preguntas);
        }
        [Route("agregar-pregunta")]
        [HttpPost]
        [Authorize(Roles = "Docente")]
        public async Task<ActionResult> SavePreguntaToPool(Pregunta pregunta)
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                int teacherId = Convert.ToInt32(claimsIdentity.FindFirst("id").Value);

                await _preguntasService.SavePreguntas(pregunta, teacherId);

                return Ok(new Response { Status = "Success", Message = "Se ha añadido la pregunta al banco de preguntas exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = ex.ToString() });
                throw;
            }
        }
    }
}

