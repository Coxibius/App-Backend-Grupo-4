using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUrbanaAPI.Data;
using TiendaUrbanaAPI.Models;

namespace TiendaUrbanaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        // ==========================================
        // EL CHEAT CODE: LLENAR CATEGORÍAS URBANA
        // ==========================================
        [HttpGet("crear-basicas")]
        public async Task<ActionResult> CrearCategoriasBasicas()
        {
            // Verificamos si ya hay datos para no duplicar
            if (await _context.Categorias.AnyAsync())
                return BadRequest("Las categorías ya fueron creadas broder.");

            var categorias = new List<Categoria>
            {
                new Categoria { NombreCategoria = "Sneakers", Descripcion = "Altas llantas, Jordan, Nike, Adidas" },
                new Categoria { NombreCategoria = "Camperas", Descripcion = "Para el frío con estilo" },
                new Categoria { NombreCategoria = "Oversize", Descripcion = "Poleras anchas para tirar facha" },
                new Categoria { NombreCategoria = "Pantalones Cargo", Descripcion = "Muchos bolsillos, mucha onda" },
                new Categoria { NombreCategoria = "Gorras y Accesorios", Descripcion = "El toque final" }
            };

            _context.Categorias.AddRange(categorias);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "¡5 Categorías urbanas creadas con éxito! Ahora el Estante 1 (Sneakers) ya existe." });
        }

        [HttpPost]
        public async Task<ActionResult<Categoria>> CrearCategoria(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }
    }
}