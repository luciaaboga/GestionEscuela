namespace GestionApiClient.Models.Dtos
{
    public class AlumnoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public Guid CursoId { get; set; }
        public string CursoNombre { get; set; } = string.Empty;
    }
}