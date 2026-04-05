using System.ComponentModel.DataAnnotations;

namespace TiendaUrbanaAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio broder.")]
        // Expresión regular: Solo acepta letras (mayúsculas/minúsculas) y espacios. NADA de números o @#$%.
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo acepta letras, no te pases de listo.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 letras.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ese no es un correo válido.")]
        public string Email { get; set; }

        public string Password { get; set; }
        
        public string DireccionEnvio { get; set; }
        
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El teléfono solo puede contener números.")]
        public string Telefono { get; set; }
    }
}