namespace GestionApiClient.Models.Dtos
{
    public class RegistrarAsistenciaDto
    {
        public Guid AlumnoId { get; set; }
        public bool Presente { get; set; }
    }
}