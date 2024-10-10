using System.ComponentModel.DataAnnotations;
using WebAppPharma.Models;

namespace WebAppPharma.Models
{
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El NOMBRE es obligatorio")]
        [StringLength(30, ErrorMessage = "El nombre no puede exceder los 30 caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Teléfono")]
        [RegularExpression(@"^[\d\s\+\-\(\)]+$", ErrorMessage = "El campo Teléfono solo puede contener números y los caracteres +, -, ()")]
        public string? Telefono { get; set; }

        //Relación de MUCHOS A MUCHOS
        public List<ProductoProveedor>? ProductosProveedores { get; set; }
    }
}
