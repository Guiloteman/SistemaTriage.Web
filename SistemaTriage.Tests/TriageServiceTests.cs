using Xunit;
using SistemaTriage.Web.Services;
using SistemaTriage.Web.Models;

namespace SistemaTriage.Tests
{
    public class TriageServiceTests
    {
        [Fact]
        public void CalcularNivelUrgencia_DebeRetornarRojo_SiSaturacionEsCritica()
        {
            // Arrange (Preparación)
            var service = new TriageService();
            var signosVitales = new Triage
            {
                SaturacionOxigeno = 85, // Crítico (< 90)
                Temperatura = 36.5m,
                FrecuenciaCardiaca = 80
            };

            // Act (Acción)
            var nivel = service.CalcularUrgencia(signosVitales);

            // Assert (Verificación)
            Assert.Equal(NivelUrgencia.Rojo, nivel);
        }
    }
}