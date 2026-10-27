using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaTriage.Web.Data;
using SistemaTriage.Web.Models;

namespace SistemaTriage.Web.Controllers
{
    [Authorize(Roles = "Medico,Admin")] // Solo médicos y administradores pueden acceder
    public class MedicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicos/Index (Cola de espera ordenada por nivel de urgencia)
        public async Task<IActionResult> Index()
        {
            // Traemos los turnos en espera incluyendo datos del paciente y del triage asociado
            var turnosEnEspera = await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Triage)
                .Where(t => t.Estado == EstadoTurno.EsperandoMedico)
                .ToListAsync();

            // Ordenamos por prioridad clínica usando el nivel de urgencia del Triage
            var turnosOrdenados = turnosEnEspera
                .OrderBy(t => t.Triage?.NivelUrgencia) // Criterio de orden automático por gravedad
                .ToList();

            return View(turnosOrdenados);
        }

        // POST: Medicos/Atender/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atender(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }

            // Cambiamos el estado del turno a Atendido
            turno.Estado = EstadoTurno.Finalizado;

            _context.Update(turno);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}