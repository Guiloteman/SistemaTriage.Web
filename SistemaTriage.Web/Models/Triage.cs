namespace SistemaTriage.Web.Models
{
    public enum NivelUrgencia
    {
        Rojo,
        Naranja,
        Amarillo,
        Verde,
        Azul
    }

    public class Triage
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public decimal Temperatura { get; set; }
        public int SaturacionOxigeno { get; set; }
        public int FrecuenciaCardiaca { get; set; }
        public NivelUrgencia NivelUrgencia { get; set; }
        public DateTime FechaEvaluacion { get; set; } = DateTime.Now;
    }
}