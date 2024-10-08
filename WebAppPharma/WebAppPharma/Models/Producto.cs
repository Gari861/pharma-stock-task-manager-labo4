using System.ComponentModel.DataAnnotations;
using System;
namespace WebAppPharma.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Display(Name = "Cod Producto")]
        public long? CodigoProducto { get; set; }

        [Required(ErrorMessage = "El TITULO es obligatorio")]
        [Display(Name = "Título")]
        public string Nombre { get; set; }

        [Display(Name = "Precio")]
        public decimal? Precio { get; set; }

        [Required(ErrorMessage = "La CANTIDAD DE PRODUCTOS es obligatoria")]
        [Display(Name = "Cantidad en Stock")]
        public int CantSock { get; set; }

        //Relación de MUCHOS A MUCHOS
        [Display(Name = "Categorías")]
        public List<ProductoCategoria>? ProductosCategorias { get; set; }

        [Display(Name = "Proveedores")]
        public List<ProductoProveedor>? ProductosProveedores { get; set; }

        [Display(Name = "Foto")]
        public string? Foto { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime? FechaIngreso { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        //Relación de UNO A UNO
        [Display(Name = "Ubicación Producto")]
        public UbicacionProducto? UbicacionProducto { get; set; }
        //LA CLAVE FORÁNEA ESTA EN EL MODELO UBICACIÓN
    }
}
