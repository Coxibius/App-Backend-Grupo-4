using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUrbanaAPI.Data;
using TiendaUrbanaAPI.Models;

namespace TiendaUrbanaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RopaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RopaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // OBTENER TODA LA ROPA (GET)
        // ==========================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ropa>>> GetRopa()
        {
            // El Include trae los datos de la categoría unida (JOIN automático)
            return await _context.Ropas.Include(r => r.Categoria).ToListAsync();
        }

        // ==========================================
        // OBTENER UNA ROPA POR ID (GET /{id})
        // ==========================================
        [HttpGet("{id}")]
        public async Task<ActionResult<Ropa>> GetRopaById(int id)
        {
            var ropa = await _context.Ropas.Include(r => r.Categoria)
                                           .FirstOrDefaultAsync(r => r.Id == id);
            if (ropa == null) return NotFound();
            return Ok(ropa);
        }

        // ==========================================
        // CREAR ROPA (POST)
        // ==========================================
        [HttpPost]
        public async Task<ActionResult<Ropa>> CrearRopa(Ropa ropa)
        {
            // Verificamos si la categoría existe antes de guardar (evita HTTP 500 por FK)
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == ropa.CategoriaId);
            if (!categoriaExiste)
            {
                return BadRequest(new { Mensaje = $"La categoría con ID {ropa.CategoriaId} no existe hermano." });
            }

            _context.Ropas.Add(ropa);
            await _context.SaveChangesAsync();
            return Ok(ropa);
        }

        // ==========================================
        // ACTUALIZAR ROPA (PUT /{id})
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarRopa(int id, Ropa ropa)
        {
            if (id != ropa.Id) return BadRequest();

            // Verificamos si la categoría existe antes de guardar
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.Id == ropa.CategoriaId);
            if (!categoriaExiste)
            {
                return BadRequest(new { Mensaje = $"La categoría con ID {ropa.CategoriaId} no existe hermano." });
            }

            _context.Entry(ropa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Ropas.AnyAsync(r => r.Id == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // ==========================================
        // ELIMINAR ROPA (DELETE /{id})
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarRopa(int id)
        {
            var ropa = await _context.Ropas.FindAsync(id);
            if (ropa == null) return NotFound();

            _context.Ropas.Remove(ropa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}