using GestionApi.Data;
using GestionApi.Models;
using GestionApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
         public CursosController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]

        public IActionResult GetAllCursos()
        {
            var allCursos = dbContext.Cursos.ToList();
            return Ok(allCursos);
        }
        
        [HttpPost]
        public IActionResult AddCurso(AddCursoDto addCursoDto)
        {
            var cursoEntity = new Curso()
            {
                Anio = addCursoDto.Anio,
                Division = addCursoDto.Division,
                Especialidad = addCursoDto.Especialidad
            };

            dbContext.Cursos.Add(cursoEntity);
            dbContext.SaveChanges();

            return Ok(cursoEntity);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteCurso(Guid id)
        {
            var curso = dbContext.Cursos.Find(id);

            if (curso == null)
            {
                return NotFound();
            }

            dbContext.Cursos.Remove(curso);
            dbContext.SaveChanges();

            return Ok();
        }
    }
}
