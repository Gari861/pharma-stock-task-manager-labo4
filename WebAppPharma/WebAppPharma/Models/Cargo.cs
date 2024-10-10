using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class Cargo
    {
        [Key]
        public int IdCargo { get; set; }

        [Required(ErrorMessage = "El CARGO es obligatorio")]
        [Display(Name = "Cargo del Empleado")]
        [StringLength(30, ErrorMessage = "El cargo no puede exceder los 30 caracteres.")]
        public string Tipo { get; set; }

        // Relación de UNO A MUCHOS
        public List<Empleado>? Empleados { get; set; }
    }
}
