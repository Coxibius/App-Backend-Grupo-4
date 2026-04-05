using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUrbanaAPI.Data;
using TiendaUrbanaAPI.Models;

namespace TiendaUrbanaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // EL CHEAT CODE: Crear un cliente VIP automático (ID 1)
        [HttpGet("crear-cliente-vip")]
        public async Task<ActionResult> CrearClienteVip()
        {
            if (await _context.Usuarios.AnyAsync())
                return BadRequest("Ya existen usuarios en la base de datos.");

            var user = new Usuario
            {
                Nombre = "El Duki",
                Email = "duki@trap.com",
                DireccionEnvio = "La Mansión",
                Telefono = "12345678"
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "¡Cliente VIP (El Duki) creado con el ID 1!" });
        }

        // Obtener todos los clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
    }
}