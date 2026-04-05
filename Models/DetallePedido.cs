using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TiendaUrbanaAPI.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public int PedidoId { get; set; }
        
        [JsonIgnore]
        [ValidateNever]
        public Pedido? Pedido { get; set; }

        // AQUÍ ACTUALIZAMOS A "Ropa"
        public int RopaId { get; set; }
        
        [JsonIgnore]
        [ValidateNever]
        public Ropa? Ropa { get; set; }
    }
}