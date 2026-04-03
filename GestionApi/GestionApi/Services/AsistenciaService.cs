using GestionApi.Data;
using GestionApi.Models.Dtos;
using GestionApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestionApi.Services
{
    public class AsistenciaService : IAsistenciaService
    {
        private readonly ApplicationDbContext _dbContext;

        public AsistenciaService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AsistenciaDto> RegistrarAsistenciaAsync(Guid cursoId, RegistrarAsistenciaDto asistenciaDto)
        {
            var fechaActual = DateOnly.FromDateTime(DateTime.Today);

            var alumno = await _dbContext.Alumnos
                .Include(a => a.Curso)
                .FirstOrDefaultAsync(a => a.Id == asistenciaDto.AlumnoId && a.CursoId == cursoId);

            if (alumno == null)
            {
                throw new Exception($"El alumno no existe o no pertenece al curso especificado.");
            }

            var asistenciaExistente = await _dbContext.Asistencias
                .FirstOrDefaultAsync(a => a.AlumnoId == asistenciaDto.AlumnoId
                    && a.CursoId == cursoId
                    && a.Fecha == fechaActual);

            if (asistenciaExistente != null)
            {
                asistenciaExistente.Presente = asistenciaDto.Presente;
                asistenciaExistente.RegistradoEn = DateTime.Now;
                await _dbContext.SaveChangesAsync();

                return new AsistenciaDto
                {
                    Id = asistenciaExistente.Id,
                    Fecha = asistenciaExistente.Fecha,
                    Presente = asistenciaExistente.Presente,
                    RegistradoEn = asistenciaExistente.RegistradoEn,
                    AlumnoId = alumno.Id,
                    AlumnoNombre = alumno.Nombre,
                    AlumnoApellido = alumno.Apellido,
                    CursoId = cursoId,
                    CursoNombre = $"{alumno.Curso.Anio}° {alumno.Curso.Division} - {alumno.Curso.Especialidad}"
                };
            }

            var asistencia = new Asistencia
            {
                Id = Guid.NewGuid(),
                Fecha = fechaActual,
                Presente = asistenciaDto.Presente,
                RegistradoEn = DateTime.Now,
                AlumnoId = asistenciaDto.AlumnoId,
                CursoId = cursoId
            };

            _dbContext.Asistencias.Add(asistencia);
            await _dbContext.SaveChangesAsync();

            return new AsistenciaDto
            {
                Id = asistencia.Id,
                Fecha = asistencia.Fecha,
                Presente = asistencia.Presente,
                RegistradoEn = asistencia.RegistradoEn,
                AlumnoId = alumno.Id,
                AlumnoNombre = alumno.Nombre,
                AlumnoApellido = alumno.Apellido,
                CursoId = cursoId,
                CursoNombre = $"{alumno.Curso.Anio}° {alumno.Curso.Division} - {alumno.Curso.Especialidad}"
            };
        }

        public async Task<List<AsistenciaDto>> GetAsistenciasByCursoYFechaAsync(Guid cursoId, DateOnly fecha)
        {
            var asistencias = await _dbContext.Asistencias
                .Include(a => a.Alumno)
                .Include(a => a.Curso)
                .Where(a => a.CursoId == cursoId && a.Fecha == fecha)
                .ToListAsync();

            return asistencias.Select(a => new AsistenciaDto
            {
                Id = a.Id,
                Fecha = a.Fecha,
                Presente = a.Presente,
                RegistradoEn = a.RegistradoEn,
                AlumnoId = a.AlumnoId,
                AlumnoNombre = a.Alumno.Nombre,
                AlumnoApellido = a.Alumno.Apellido,
                CursoId = a.CursoId,
                CursoNombre = $"{a.Curso.Anio}° {a.Curso.Division} - {a.Curso.Especialidad}"
            }).ToList();
        }

        public async Task<ResumenAsistenciaDto> GetResumenAsistenciaAsync(Guid cursoId, DateOnly fecha)
        {
            var alumnosCurso = await _dbContext.Alumnos
                .Where(a => a.CursoId == cursoId)
                .ToListAsync();

            var totalAlumnos = alumnosCurso.Count;

            var asistencias = await _dbContext.Asistencias
                .Where(a => a.CursoId == cursoId && a.Fecha == fecha)
                .ToListAsync();

            var presentes = asistencias.Count(a => a.Presente);
            var ausentes = totalAlumnos - presentes; 

            return new ResumenAsistenciaDto
            {
                Fecha = fecha,
                TotalAlumnos = totalAlumnos,
                Presentes = presentes,
                Ausentes = ausentes
            };
        }

        public async Task<List<AsistenciaDto>> GetAsistenciasHoyAsync(Guid cursoId)
        {
            var fechaHoy = DateOnly.FromDateTime(DateTime.Today);
            return await GetAsistenciasByCursoYFechaAsync(cursoId, fechaHoy);
        }

        public async Task<ResumenAsistenciaDto> GetResumenHoyAsync(Guid cursoId)
        {
            var fechaHoy = DateOnly.FromDateTime(DateTime.Today);
            return await GetResumenAsistenciaAsync(cursoId, fechaHoy);
        }
    }
}