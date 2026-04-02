using GestionApi.Data;
using GestionApi.Models.Dtos;
using GestionApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionApi.Services
{
    public class AlumnoService : IAlumnoService
    {
        private readonly ApplicationDbContext _dbContext;

        public AlumnoService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AlumnoDto>> GetAlumnosByCursoIdAsync(Guid cursoId)
        {
            var alumnos = await _dbContext.Alumnos
                .Include(a => a.Curso)
                .Where(a => a.CursoId == cursoId)
                .ToListAsync();

            return alumnos.Select(a => new AlumnoDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                Email = a.Email,
                FechaRegistro = a.FechaRegistro,
                CursoId = a.CursoId,
                CursoNombre = a.Curso != null ? $"{a.Curso.Anio}° {a.Curso.Division} - {a.Curso.Especialidad}" : string.Empty
            }).ToList();
        }

        public async Task<AlumnoDto> AddAlumnoToCursoAsync(Guid cursoId, AddAlumnoToCursoDto addAlumnoDto)
        {
            var curso = await _dbContext.Cursos.FindAsync(cursoId);
            if (curso == null)
            {
                throw new Exception($"No se encontró el curso con ID {cursoId}");
            }

            var emailExiste = await _dbContext.Alumnos.AnyAsync(a => a.Email == addAlumnoDto.Email);
            if (emailExiste)
            {
                throw new Exception($"Ya existe un alumno con el email {addAlumnoDto.Email}");
            }

            var alumnoEntity = new Alumno
            {
                Id = Guid.NewGuid(),
                Nombre = addAlumnoDto.Nombre,
                Apellido = addAlumnoDto.Apellido,
                Email = addAlumnoDto.Email,
                CursoId = cursoId,
                FechaRegistro = DateTime.UtcNow
            };

            _dbContext.Alumnos.Add(alumnoEntity);
            await _dbContext.SaveChangesAsync();

            return new AlumnoDto
            {
                Id = alumnoEntity.Id,
                Nombre = alumnoEntity.Nombre,
                Apellido = alumnoEntity.Apellido,
                Email = alumnoEntity.Email,
                FechaRegistro = alumnoEntity.FechaRegistro,
                CursoId = alumnoEntity.CursoId,
                CursoNombre = $"{curso.Anio}° {curso.Division} - {curso.Especialidad}"
            };
        }

        public async Task<bool> DeleteAlumnoAsync(Guid id)
        {
            var alumno = await _dbContext.Alumnos.FindAsync(id);

            if (alumno == null)
            {
                return false;
            }

            _dbContext.Alumnos.Remove(alumno);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<AlumnoDto?> GetAlumnoByIdAsync(Guid id)
        {
            var alumno = await _dbContext.Alumnos
                .Include(a => a.Curso)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (alumno == null)
            {
                return null;
            }

            return new AlumnoDto
            {
                Id = alumno.Id,
                Nombre = alumno.Nombre,
                Apellido = alumno.Apellido,
                Email = alumno.Email,
                FechaRegistro = alumno.FechaRegistro,
                CursoId = alumno.CursoId,
                CursoNombre = alumno.Curso != null ? $"{alumno.Curso.Anio}° {alumno.Curso.Division} - {alumno.Curso.Especialidad}" : string.Empty
            };
        }
    }
}