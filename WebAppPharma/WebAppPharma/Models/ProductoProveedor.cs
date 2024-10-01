using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppPharma.Models;

namespace WebAppPharma.Models
{
    public class ProductoProveedor
    {
        //Relación de MUCHOS A MUCHOS
        public int IdProducto { get; set; }
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }

        public int IdProveedor { get; set; }
        [ForeignKey(nameof(IdProveedor))]
        public Proveedor? Proveedor { get; set; }
    }
}
