using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPharma.Models
{
    public class UbicacionProducto
    {
        [Key]
        public int IdUbicacion { get; set; }

        [Display(Name = "Estante")]
        public string? Estante { get; set; }

        [Required(ErrorMessage = "La SECCION es obligatoria")]
        [Display(Name = "Sección")]
        public string Seccion { get; set; }

        [Display(Name = "Pasillo")]
        public string? Pasillo { get; set; }

        //Relación de UNO A UNO
        [Required(ErrorMessage = "El PRODUCTO es obligatorio")]
        [Display(Name = "Producto")]
        public int IdProducto { get; set; }
        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }
    }
}
