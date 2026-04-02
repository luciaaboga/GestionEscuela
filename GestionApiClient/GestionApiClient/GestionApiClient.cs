using System.Text;
using System.Text.Json;
using GestionApiClient.Exceptions;
using GestionApiClient.Models.Dtos;

namespace GestionApiClient
{
    public class GestionApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private bool _disposed;

        public GestionApiClient(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        // ============ MÉTODOS PARA CURSOS ============
        public async Task<List<CursoDto>> GetAllCursosAsync()
        {
            var response = await _httpClient.GetAsync("/api/cursos");

            await HandleErrorResponse(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CursoDto>>(json, _jsonOptions)
                   ?? new List<CursoDto>();
        }

        public async Task<CursoDto> CreateCursoAsync(AddCursoDto curso)
        {
            var json = JsonSerializer.Serialize(curso, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/cursos", content);

            await HandleErrorResponse(response);

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CursoDto>(responseJson, _jsonOptions)
                   ?? throw new ApiException("Error al deserializar respuesta", 500);
        }
        public async Task<bool> DeleteCursoAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/cursos/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }

            await HandleErrorResponse(response);

            return response.IsSuccessStatusCode;
        }

        private async Task HandleErrorResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var statusCode = (int)response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            throw new ApiException(
                $"Error en la API: {response.ReasonPhrase}",
                statusCode,
                responseBody);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _httpClient?.Dispose();
            }
            _disposed = true;
        }

        // ============ MÉTODOS PARA ALUMNOS ============

        public async Task<List<AlumnoDto>> GetAlumnosByCursoIdAsync(Guid cursoId)
        {
            var response = await _httpClient.GetAsync($"/api/alumnos/curso/{cursoId}");

            await HandleErrorResponse(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AlumnoDto>>(json, _jsonOptions)
                   ?? new List<AlumnoDto>();
        }

        public async Task<AlumnoDto> AddAlumnoToCursoAsync(Guid cursoId, AddAlumnoToCursoDto alumno)
        {
            var json = JsonSerializer.Serialize(alumno, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"/api/alumnos/curso/{cursoId}", content);

            await HandleErrorResponse(response);

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlumnoDto>(responseJson, _jsonOptions)
                   ?? throw new ApiException("Error al deserializar respuesta", 500);
        }

        public async Task<bool> DeleteAlumnoAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/alumnos/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }

            await HandleErrorResponse(response);

            return response.IsSuccessStatusCode;
        }
        public async Task<AlumnoDto?> GetAlumnoByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/alumnos/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            await HandleErrorResponse(response);

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AlumnoDto>(json, _jsonOptions);
        }
    }
}