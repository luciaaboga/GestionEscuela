using GestionApi.Models.Dtos;
using GestionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly IAlumnoService _alumnoService;

        public AlumnosController(IAlumnoService alumnoService)
        {
            _alumnoService = alumnoService;
        }

        [HttpGet("curso/{cursoId:guid}")]
        public async Task<IActionResult> GetAlumnosByCurso(Guid cursoId)
        {
            var alumnos = await _alumnoService.GetAlumnosByCursoIdAsync(cursoId);
            return Ok(alumnos);
        }

        [HttpPost("curso/{cursoId:guid}")]
        public async Task<IActionResult> AddAlumnoToCurso(Guid cursoId, AddAlumnoToCursoDto addAlumnoDto)
        {
            try
            {
                var nuevoAlumno = await _alumnoService.AddAlumnoToCursoAsync(cursoId, addAlumnoDto);
                return CreatedAtAction(nameof(GetAlumnosByCurso), new { cursoId = cursoId }, nuevoAlumno);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAlumno(Guid id)
        {
            var result = await _alumnoService.DeleteAlumnoAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAlumnoById(Guid id)
        {
            var alumno = await _alumnoService.GetAlumnoByIdAsync(id);

            if (alumno == null)
            {
                return NotFound();
            }

            return Ok(alumno);
        }
    }
}