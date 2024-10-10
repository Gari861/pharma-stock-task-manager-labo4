using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPharma.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El NOMBRE es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(20, ErrorMessage = "El nombre no puede exceder los 20 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El APELLIDO es obligatorio")]
        [Display(Name = "Apellido")]
        [StringLength(20, ErrorMessage = "El apellido no puede exceder los 20 caracteres.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [Display(Name = "DNI")]
        [RegularExpression(@"^[0-9.-]+$", ErrorMessage = "El DNI solo puede contener números, puntos y guiones.")]
        public string Dni { get; set; }

        //Relación de UNO A MUCHOS
        [Display(Name = "Cargo")]
        public int? IdCargo { get; set; }
        [ForeignKey(nameof(IdCargo))]
        public Cargo? Cargo { get; set; }
        public int? IdEstadodeEmpleado { get; set; }
        [ForeignKey(nameof(IdEstadodeEmpleado))]
        [Display(Name = "Estado de empleado")]
        public EstadodeEmpleado? EstadodeEmpleado { get; set; }

        [Required(ErrorMessage = "El TELEFONO es obligatorio")]
        [Display(Name = "Telefono")]
        [RegularExpression(@"^[\d\s\+\-\(\)]+$", ErrorMessage = "El campo Teléfono solo puede contener números y los caracteres +, -, ()")]
        public string Telefono { get; set; }

        [Display(Name = "Fecha de Nacimiento ")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Foto")]
        public string? Foto { get; set; }

        //Relación MUCHOS A MUCHOS
        [Display(Name = "Asignacion")]
        public List<TareaEmpleado>? TareasEmpleados { get; set; }
    }
}
