namespace GestionApi.Models.Dtos
{
    public class AsistenciaDto
    {
        public Guid Id { get; set; }
        public DateOnly Fecha { get; set; }
        public bool Presente { get; set; }
        public string Estado => Presente ? "Presente" : "Ausente";
        public DateTime RegistradoEn { get; set; }
        public Guid AlumnoId { get; set; }
        public string AlumnoNombre { get; set; } = string.Empty;
        public string AlumnoApellido { get; set; } = string.Empty;
        public string AlumnoNombreCompleto => $"{AlumnoApellido}, {AlumnoNombre}";
        public Guid CursoId { get; set; }
        public string CursoNombre { get; set; } = string.Empty;
    }
}