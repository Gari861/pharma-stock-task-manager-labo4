using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPharma.Models
{
    public class ProductoCategoria
    {
        [Key]
        public int IdProductoCategoria { get; set; }

        //Relación de MUCHOS A MUCHOS
        public int IdProducto { get; set; }
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }

        public int IdProveedor { get; set; }
        [ForeignKey(nameof(IdProveedor))]
        public Proveedor? Proveedor { get; set; }
    }
}
