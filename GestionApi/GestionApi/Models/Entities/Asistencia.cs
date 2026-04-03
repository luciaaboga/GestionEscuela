namespace GestionApi.Models.Entities
{
    public class Asistencia
    {
        public Guid Id { get; set; }
        public DateOnly Fecha { get; set; } 
        public bool Presente { get; set; } 
        public DateTime RegistradoEn { get; set; } 
        public Guid AlumnoId { get; set; }
        public Guid CursoId { get; set; }
        public Alumno Alumno { get; set; } = null!;
        public Curso Curso { get; set; } = null!;
    }
}