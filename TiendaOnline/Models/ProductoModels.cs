using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaOnline.Models
{
    public class ProductoModels
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required, StringLength(80)]
        [Display(Name = "Nombre del Producto")]
        public string Nombre { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Precio del Producto")]
        public decimal Precio { get; set; }

        [Display(Name = "Disponibilidad en Inventario")]
        [Range(1, 1000)]
        public int Disponibilidad { get; set; }

        public byte[] Imagen { get; set; }

        [Display(Name = "Estado")]
        public EstadoProductoModels Estado { get; set; } = new EstadoProductoModels();

        [Required, StringLength(80)]
        [Display(Name = "Reseñas")]
        public string Resenia { get; set; }

        public virtual ICollection<CompraModels> Compras { get; set; } = new List<CompraModels>();
    }
}




