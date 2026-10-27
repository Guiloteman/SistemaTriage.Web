using SistemaTriage.Web.Models;

namespace SistemaTriage.Web.Services
{
    public class TriageService
    {
        public NivelUrgencia CalcularUrgencia(Triage triage)
        {
            // Lógica preliminar para hacer pasar el test (Fase Verde)
            if (triage.SaturacionOxigeno < 90)
            {
                return NivelUrgencia.Rojo;
            }

            return NivelUrgencia.Verde; // Valor por defecto temporal
        }
    }
}