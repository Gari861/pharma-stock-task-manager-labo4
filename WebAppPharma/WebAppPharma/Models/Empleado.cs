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
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El APELLIDO es obligatorio")]
        [Display(Name = "Apellido")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [Display(Name = "DNI")]
        public long Dni { get; set; }

        //Relación de UNO A MUCHOS
        [Required(ErrorMessage = "El CARGO es obligatorio")]
        [Display(Name = "Cargo")]
        public int IdCargo { get; set; }
        [ForeignKey(nameof(IdCargo))]
        public Cargo? Cargo { get; set; }

        [Required(ErrorMessage = "El ESTADO es obligatorio")]
        [Display(Name = "Estado")]
        public int IdEstadodeEmpleado { get; set; }
        [ForeignKey(nameof(IdEstadodeEmpleado))]
        public EstadodeEmpleado? EstadodeEmpleado { get; set; }

        [Required(ErrorMessage = "El TELEFONO es obligatorio")]
        [Display(Name = "Telefono")]
        public long Telefono { get; set; }

        [Required(ErrorMessage = "La FECHA DE NACIMIENTO es obligatoria")]
        [Display(Name = "Fecha de Nacimiento ")]
        public DateTime FechaNacimiento { get; set; }

        [Display(Name = "Foto")]
        public string? Foto { get; set; }

        //Relación MUCHOS A MUCHOS
        [Display(Name = "Asignacion")]
        public List<TareaEmpleado>? TareasEmpleados { get; set; }
    }
}
