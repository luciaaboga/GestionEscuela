namespace GestionApi.Models.Dtos
{
    public class AddAlumnoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid CursoId { get; set; }
    }
}