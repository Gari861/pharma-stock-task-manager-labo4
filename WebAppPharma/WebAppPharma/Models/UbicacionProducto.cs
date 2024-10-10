using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPharma.Models
{
    public class UbicacionProducto
    {
        [Key]
        public int IdUbicacion { get; set; }

        [Display(Name = "Estante")]
        [StringLength(10, ErrorMessage = "El estante no puede exceder los 10 caracteres.")]
        public string? Estante { get; set; }

        [Required(ErrorMessage = "La SECCION es obligatoria")]
        [Display(Name = "Sección")]
        [StringLength(10, ErrorMessage = "La sección no puede exceder los 10 caracteres.")]
        public string Seccion { get; set; }

        [Display(Name = "Pasillo")]
        [StringLength(10, ErrorMessage = "El pasillo no puede exceder los 10 caracteres.")]
        public string? Pasillo { get; set; }

        //Relación de UNO A UNO
        [Required(ErrorMessage = "El PRODUCTO es obligatorio")]
        [Display(Name = "Producto")]
        public int IdProducto { get; set; }
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }
    }
}
