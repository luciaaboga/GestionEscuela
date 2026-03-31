using GestionApi.Models;
using GestionApi.Models.Dtos;

namespace GestionApi.Services
{
    public interface ICursosService
    {
        Task<List<CursoDto>> GetAllCursosAsync();
        Task<CursoDto> AddCursoAsync(AddCursoDto addCursoDto);
        Task<bool> DeleteCursoAsync(Guid id);
    }
}