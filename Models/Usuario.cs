using System.ComponentModel.DataAnnotations;

namespace TiendaUrbanaAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        // Bajando los estandares como lo solicitaste xD
        // Quitamos los Regex estrictos que rompían la validación del ModelState
        
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; }

        // Campos opcionales para evitar que la API rechace por falta de datos
        public string? Password { get; set; }
        public string? DireccionEnvio { get; set; }
        public string? Telefono { get; set; }
    }
}