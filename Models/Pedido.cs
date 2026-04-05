using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TiendaUrbanaAPI.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Estado { get; set; } // Ej: "Pagado", "En_Ruta"

        // Relación: El usuario que compró
        public int UsuarioId { get; set; }
        
        [JsonIgnore]
        [ValidateNever]
        public Usuario? Usuario { get; set; }

        // Relación: Las prendas que compró
        [ValidateNever]
        public ICollection<DetallePedido>? Detalles { get; set; }
    }
}