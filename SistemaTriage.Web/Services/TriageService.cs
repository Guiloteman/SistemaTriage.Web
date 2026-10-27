using SistemaTriage.Web.Models;

namespace SistemaTriage.Web.Services
{
    public class TriageService
    {
        public NivelUrgencia CalcularUrgencia(Triage triage)
        {
            // Criterios de Emergencia (Rojo)
            if (triage.SaturacionOxigeno < 90 || triage.Temperatura > 40.0m || triage.FrecuenciaCardiaca > 140 || triage.FrecuenciaCardiaca < 40)
            {
                return NivelUrgencia.Rojo;
            }

            // Criterios de Urgencia Alta (Naranja)
            if ((triage.SaturacionOxigeno >= 90 && triage.SaturacionOxigeno <= 92) || triage.Temperatura >= 38.5m || triage.FrecuenciaCardiaca > 120)
            {
                return NivelUrgencia.Naranja;
            }

            // Criterios de Urgencia Moderada (Amarillo)
            if (triage.Temperatura >= 37.5m || (triage.FrecuenciaCardiaca >= 100 && triage.FrecuenciaCardiaca <= 120))
            {
                return NivelUrgencia.Amarillo;
            }

            // Criterios Leves (Verde / Azul)
            return NivelUrgencia.Verde;
        }
    }
}