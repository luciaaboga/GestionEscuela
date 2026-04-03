namespace GestionApi.Models.Dtos
{
    public class ResumenAsistenciaDto
    {
        public DateOnly Fecha { get; set; }
        public int TotalAlumnos { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public double PorcentajePresentes => TotalAlumnos > 0 ? (double)Presentes / TotalAlumnos * 100 : 0;
        public double PorcentajeAusentes => TotalAlumnos > 0 ? (double)Ausentes / TotalAlumnos * 100 : 0;
    }
}