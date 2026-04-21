using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class CompraModels
    {
        public int Id { get; set; }

        // Relación con Producto
        [Required]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public virtual ProductoModels Producto { get; set; }

        // Usuario que compra (ASP.NET Identity)
        [Required]
        public string UsuarioId { get; set; }

        // Precio al momento de la compra
        [Required]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        // Cantidad comprada
        [Required]
        [Range(1, 100)]
        public int Cantidad { get; set; }

        // Fecha de compra
        [Required]
        public DateTime FechaCompra { get; set; } = DateTime.Now;
    }
}