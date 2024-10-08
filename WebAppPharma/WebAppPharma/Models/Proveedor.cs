using System.ComponentModel.DataAnnotations;
using WebAppPharma.Models;

namespace WebAppPharma.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El NOMBRE es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        public string? Apellido { get; set; }

        [Display(Name = "Telefono")]
        public long? Telefono { get; set; }

        //Relación de MUCHOS A MUCHOS
        public List<ProductoProveedor>? ProductosProveedores { get; set; }
    }
}
