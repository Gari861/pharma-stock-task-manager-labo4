using System.ComponentModel.DataAnnotations;
using System;
namespace WebAppPharma.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El CODIGO DEL PRODUCTO es obligatorio")]
        [Display(Name = "Codigo Producto")]
        public long CodigoProducto { get; set; }

        [Required(ErrorMessage = "El TITULO es obligatorio")]
        [Display(Name = "Título")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El PRECIO es obligatorio")]
        [Display(Name = "Precio")]
        public int Precio { get; set; }

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

        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaVencimiento { get; set; }

        //Relación de UNO A UNO
        [Display(Name = "Ubicación Producto")]
        public UbicacionProducto? UbicacionProducto { get; set; }
        //LA CLAVE FORÁNEA ESTA EN LA CLASE UBICACIÓN
    }
}
