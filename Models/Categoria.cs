using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TiendaUrbanaAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        public string NombreCategoria { get; set; }

        public string Descripcion { get; set; }

        // AQUÍ TAMBIÉN LE PONEMOS EL SIGNO DE INTERROGACIÓN
        [JsonIgnore]
        public ICollection<Ropa>? Ropas { get; set; }
    }
}