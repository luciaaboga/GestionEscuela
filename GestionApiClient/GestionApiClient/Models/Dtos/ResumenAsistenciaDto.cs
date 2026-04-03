namespace GestionApiClient.Models.Dtos
{
    public class ResumenAsistenciaDto
    {
        public DateOnly Fecha { get; set; }
        public int TotalAlumnos { get; set; }
        public int Presentes { get; set; }
        public int Ausentes { get; set; }
        public double PorcentajePresentes { get; set; }
        public double PorcentajeAusentes { get; set; }
    }
}