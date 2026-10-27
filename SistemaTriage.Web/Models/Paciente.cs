namespace SistemaTriage.Web.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; } = string.Empty;

        // Relaciones de navegación
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();
        public ICollection<Triage> Triages { get; set; } = new List<Triage>();
    }
}