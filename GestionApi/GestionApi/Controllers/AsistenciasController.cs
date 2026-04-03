using GestionApi.Models.Dtos;
using GestionApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciasController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;

        public AsistenciasController(IAsistenciaService asistenciaService)
        {
            _asistenciaService = asistenciaService;
        }

        [HttpPost("curso/{cursoId:guid}/registrar")]
        public async Task<IActionResult> RegistrarAsistencia(Guid cursoId, RegistrarAsistenciaDto asistenciaDto)
        {
            try
            {
                var asistencia = await _asistenciaService.RegistrarAsistenciaAsync(cursoId, asistenciaDto);
                return Ok(asistencia);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("curso/{cursoId:guid}/fecha/{fecha}")]
        public async Task<IActionResult> GetAsistenciasByFecha(Guid cursoId, DateOnly fecha)
        {
            var asistencias = await _asistenciaService.GetAsistenciasByCursoYFechaAsync(cursoId, fecha);
            return Ok(asistencias);
        }

        [HttpGet("curso/{cursoId:guid}/resumen")]
        public async Task<IActionResult> GetResumenAsistencia(Guid cursoId, [FromQuery] DateOnly fecha)
        {
            var resumen = await _asistenciaService.GetResumenAsistenciaAsync(cursoId, fecha);
            return Ok(resumen);
        }

        [HttpGet("curso/{cursoId:guid}/hoy")]
        public async Task<IActionResult> GetAsistenciasHoy(Guid cursoId)
        {
            var asistencias = await _asistenciaService.GetAsistenciasHoyAsync(cursoId);
            return Ok(asistencias);
        }

        [HttpGet("curso/{cursoId:guid}/resumen-hoy")]
        public async Task<IActionResult> GetResumenHoy(Guid cursoId)
        {
            var resumen = await _asistenciaService.GetResumenHoyAsync(cursoId);
            return Ok(resumen);
        }
    }
}