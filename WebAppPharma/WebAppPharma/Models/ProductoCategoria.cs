using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppPharma.Models;

namespace WebAppPharma.Models
{
    public class ProductoCategoria
    {
        //Relación de MUCHOS A MUCHOS
        public int IdProducto { get; set; }
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }

        public int IdCategoria { get; set; }
        [ForeignKey(nameof(IdCategoria))]
        public Categoria? Categoria { get; set; }
    }
}
