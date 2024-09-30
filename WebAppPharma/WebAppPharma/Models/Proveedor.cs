using System.ComponentModel.DataAnnotations;
using WebAppLibros.Models;

namespace WebAppPharma.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El NOMBRE es obligatorio")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El APELLIDO es obligatorio")]
        [Display(Name = "Apellido")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El TELEFONO es obligatorio")]
        [Display(Name = "Telefono")]
        public int Telefono { get; set; }

        //Relación de MUCHOS A MUCHOS
        public List<ProductoProveedor>? ProductosProveedores { get; set; }
    }
}
