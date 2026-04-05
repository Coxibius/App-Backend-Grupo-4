using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUrbanaAPI.Data;
using TiendaUrbanaAPI.Models;

namespace TiendaUrbanaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PedidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ver todas las ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            // Magia de EF Core: Traemos el pedido + los datos del usuario + los detalles de la compra
            return await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Detalles)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Pedido>> CrearPedido(Pedido pedido)
        {
            // Validar que el usuario exista
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == pedido.UsuarioId);
            if (!usuarioExiste)
            {
                return BadRequest(new { Mensaje = $"El usuario con ID {pedido.UsuarioId} no existe bro." });
            }

            // El Total y la Fecha se pueden calcular aquí, pero por ahora solo guardamos
            pedido.Fecha = DateTime.Now;
            pedido.Estado = "Pagado y Preparando Envío";

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }
    }
}