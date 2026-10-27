using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaTriage.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/Index (Lista de usuarios y sus roles)
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var modeloUsuarios = new List<UsuarioConRolesViewModel>();

            foreach (var user in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(user);
                modeloUsuarios.Add(new UsuarioConRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    Roles = roles.ToList()
                });
            }

            return View(modeloUsuarios);
        }

        // GET: Admin/EditRoles/5
        public async Task<IActionResult> EditRoles(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            var rolesDisponibles = await _roleManager.Roles.Select(r => r.Name!).ToListAsync();
            var rolesActuales = await _userManager.GetRolesAsync(usuario);

            var modelo = new EditarRolesViewModel
            {
                UserId = usuario.Id,
                Email = usuario.Email ?? string.Empty,
                RolesAsignados = rolesDisponibles.Select(role => new RolSeleccionViewModel
                {
                    NombreRol = role,
                    Seleccionado = rolesActuales.Contains(role)
                }).ToList()
            };

            return View(modelo);
        }

        // POST: Admin/EditRoles/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditarRolesViewModel modelo)
        {
            var usuario = await _userManager.FindByIdAsync(modelo.UserId);
            if (usuario == null) return NotFound();

            var rolesActuales = await _userManager.GetRolesAsync(usuario);
            var rolesSeleccionados = modelo.RolesAsignados.Where(r => r.Seleccionado).Select(r => r.NombreRol).ToList();

            // Quitamos los roles que ya no están seleccionados
            var aQuitar = rolesActuales.Except(rolesSeleccionados);
            await _userManager.RemoveFromRolesAsync(usuario, aQuitar);

            // Agregamos los nuevos roles seleccionados
            var aAgregar = rolesSeleccionados.Except(rolesActuales);
            await _userManager.AddToRolesAsync(usuario, aAgregar);

            return RedirectToAction(nameof(Index));
        }
    }

    // ViewModels auxiliares para el panel
    public class UsuarioConRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
    }

    public class EditarRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<RolSeleccionViewModel> RolesAsignados { get; set; } = new();
    }

    public class RolSeleccionViewModel
    {
        public string NombreRol { get; set; } = string.Empty;
        public bool Seleccionado { get; set; }
    }
}