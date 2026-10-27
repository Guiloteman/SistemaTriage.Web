using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaTriage.Web.Models;

namespace SistemaTriage.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Triage> Triages { get; set; }
        public DbSet<Turno> Turnos { get; set; }
    }
}
