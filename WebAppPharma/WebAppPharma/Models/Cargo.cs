using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class Cargo
    {
        [Key]
        public int IdCargo { get; set; }

        [Required(ErrorMessage = "El CARGO es obligatoria")]
        [Display(Name = "Cargo del Empleado")]
        public string? Tipo { get; set; }

        // Relación de UNO A MUCHOS
        public List<Empleado>? Empleados { get; set; }
    }
}
