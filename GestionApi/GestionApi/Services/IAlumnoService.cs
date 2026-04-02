using GestionApi.Models.Dtos;

namespace GestionApi.Services
{
    public interface IAlumnoService
    {
        Task<List<AlumnoDto>> GetAlumnosByCursoIdAsync(Guid cursoId);

        Task<AlumnoDto> AddAlumnoToCursoAsync(Guid cursoId, AddAlumnoToCursoDto addAlumnoDto);

        Task<bool> DeleteAlumnoAsync(Guid id);

        Task<AlumnoDto?> GetAlumnoByIdAsync(Guid id);
    }
}