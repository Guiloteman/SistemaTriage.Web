namespace SistemaTriage.Web.Models
{
    public enum EstadoTurno
    {
        RegistradoEnEspera,
        EnTriage,
        EsperandoMedico,
        EnAtencion,
        Finalizado,
        Cancelado
    }

    public class Turno
    {
        public int Id { get; set; }

        public int PacienteId { get; set; }
        public Paciente? Paciente { get; set; }

        public int? TriageId { get; set; }
        public Triage? Triage { get; set; }

        public EstadoTurno Estado { get; set; } = EstadoTurno.RegistradoEnEspera;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string? ConsultorioAsignado { get; set; }
    }
}