using GestionApi.Models.Dtos;

namespace GestionApi.Services
{
    public interface IAsistenciaService
    {
        Task<AsistenciaDto> RegistrarAsistenciaAsync(Guid cursoId, RegistrarAsistenciaDto asistenciaDto);

        Task<List<AsistenciaDto>> GetAsistenciasByCursoYFechaAsync(Guid cursoId, DateOnly fecha);

        Task<ResumenAsistenciaDto> GetResumenAsistenciaAsync(Guid cursoId, DateOnly fecha);

        Task<List<AsistenciaDto>> GetAsistenciasHoyAsync(Guid cursoId);

        Task<ResumenAsistenciaDto> GetResumenHoyAsync(Guid cursoId);
    }
}