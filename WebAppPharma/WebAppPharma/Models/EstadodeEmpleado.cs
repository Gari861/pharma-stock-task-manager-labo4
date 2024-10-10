using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class EstadodeEmpleado
    {
        [Key]
        public int IdEstadodeEmpleado { get; set; }

        [Required(ErrorMessage = "El ESTADO es obligatorio")]
        [Display(Name = "Estado del Empleado")]
        [StringLength(20, ErrorMessage = "El estado de empleado no puede exceder los 20 caracteres.")]
        public string Tipo { get; set; }

        //Relación de UNO A MUCHOS
        public List<Empleado>? Empleados { get; set; }
    }
}
