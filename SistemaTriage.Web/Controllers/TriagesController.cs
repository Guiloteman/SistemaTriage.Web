using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaTriage.Web.Data;
using SistemaTriage.Web.Models;
using SistemaTriage.Web.Services;

namespace SistemaTriage.Web.Controllers
{
    [Authorize(Roles = "Enfermero,Medico, Admin")]
    public class TriagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TriageService _triageService;

        public TriagesController(ApplicationDbContext context, TriageService triageService)
        {
            _context = context;
            _triageService = triageService;
        }

        // GET: Triages/Create
        public IActionResult Create(int? pacienteId)
        {
            if (pacienteId == null)
            {
                // Si no viene por parámetro, mostramos una lista desplegable con todos los pacientes
                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Dni");
            }
            else
            {
                ViewData["PacienteId"] = pacienteId;
                ViewBag.PacienteNombre = _context.Pacientes.Where(p => p.Id == pacienteId)
                    .Select(p => p.Nombre + " " + p.Apellido).FirstOrDefault();
            }

            return View();
        }

        // POST: Triages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PacienteId,Temperatura,SaturacionOxigeno,FrecuenciaCardiaca")] Triage triage)
        {
            // Calculamos automáticamente el nivel de urgencia usando nuestro servicio
            triage.NivelUrgencia = _triageService.CalcularUrgencia(triage);
            triage.FechaEvaluacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                // 1. Guardamos el Triage
                _context.Add(triage);
                await _context.SaveChangesAsync();

                // 2. Generamos automáticamente un Turno asociado en espera para el médico
                var turno = new Turno
                {
                    PacienteId = triage.PacienteId,
                    TriageId = triage.Id,
                    Estado = EstadoTurno.EsperandoMedico,
                    FechaCreacion = DateTime.Now
                };

                _context.Add(turno);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Pacientes");
            }

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Dni", triage.PacienteId);
            return View(triage);
        }
    }
}