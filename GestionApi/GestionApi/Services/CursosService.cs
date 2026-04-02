using GestionApi.Data;
using GestionApi.Models.Dtos;
using GestionApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionApi.Services
{
    public class CursosService : ICursosService
    {
        private readonly ApplicationDbContext _dbContext;

        public CursosService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CursoDto>> GetAllCursosAsync()
        {
            var cursos = await _dbContext.Cursos.ToListAsync();

            return cursos.Select(c => new CursoDto
            {
                Id = c.Id,
                Anio = c.Anio,
                Division = c.Division,
                Especialidad = c.Especialidad
            }).ToList();
        }

        public async Task<CursoDto> AddCursoAsync(AddCursoDto addCursoDto)
        {
            var cursoEntity = new Curso
            {
                Id = Guid.NewGuid(),
                Anio = addCursoDto.Anio,
                Division = addCursoDto.Division,
                Especialidad = addCursoDto.Especialidad
            };

            _dbContext.Cursos.Add(cursoEntity);
            await _dbContext.SaveChangesAsync();

            return new CursoDto
            {
                Id = cursoEntity.Id,
                Anio = cursoEntity.Anio,
                Division = cursoEntity.Division,
                Especialidad = cursoEntity.Especialidad
            };
        }

        public async Task<bool> DeleteCursoAsync(Guid id)
        {
            var curso = await _dbContext.Cursos.FindAsync(id);

            if (curso == null)
            {
                return false;
            }

            _dbContext.Cursos.Remove(curso);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}