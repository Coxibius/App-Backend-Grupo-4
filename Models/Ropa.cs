using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // <- LA ORDEN JUDICIAL

namespace TiendaUrbanaAPI.Models
{
    public class Ropa
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public string Talla { get; set; }
        public string Color { get; set; }
        public int Stock { get; set; }

        public string? ImagenUrl { get; set; } 

        public int CategoriaId { get; set; }

        [JsonIgnore]
        [ValidateNever] // <- EL TIRO DE GRACIA AL GUARDIA
        public Categoria? Categoria { get; set; }
    }
}