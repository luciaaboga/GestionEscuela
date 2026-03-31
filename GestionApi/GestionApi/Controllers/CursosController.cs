using GestionApi.Models;
using GestionApi.Models.Dtos;
using GestionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ICursosService _cursosService;

        public CursosController(ICursosService cursosService)
        {
            _cursosService = cursosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCursos()
        {
            var cursos = await _cursosService.GetAllCursosAsync();
            return Ok(cursos);
        }

        [HttpPost]
        public async Task<IActionResult> AddCurso(AddCursoDto addCursoDto)
        {
            var nuevoCurso = await _cursosService.AddCursoAsync(addCursoDto);
            return CreatedAtAction(nameof(GetAllCursos), new { id = nuevoCurso.Id }, nuevoCurso);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCurso(Guid id)
        {
            var result = await _cursosService.DeleteCursoAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent(); 
        }
    }
}