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
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El APELLIDO es obligatorio")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [Display(Name = "DNI")]
        public long Dni { get; set; }

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
        public long Telefono { get; set; }

        [Display(Name = "Fecha de Nacimiento ")]
        public DateTime? FechaNacimiento { get; set; }

        [Display(Name = "Foto")]
        public string? Foto { get; set; }

        //Relación MUCHOS A MUCHOS
        [Display(Name = "Asignacion")]
        public List<TareaEmpleado>? TareasEmpleados { get; set; }
    }
}
