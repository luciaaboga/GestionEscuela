namespace GestionApiClient.Models.Dtos
{
    public class CursoDto
    {
        public Guid Id { get; set; }
        public string Anio { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
    }
}