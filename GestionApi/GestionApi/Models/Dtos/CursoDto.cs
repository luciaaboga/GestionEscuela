namespace GestionApi.Models.Dtos
{
    public class CursoDto
    {
        public Guid Id { get; set; }
        public string Anio { get; set; }
        public string Division { get; set; }
        public string Especialidad { get; set; }
        public string NombreCompleto => $"{Anio}° {Division} - {Especialidad}";
    }
}