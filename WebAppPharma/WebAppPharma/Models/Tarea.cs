using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class Tarea
    {
        [Key]
        public int IdTarea { get; set; }

        [Required(ErrorMessage = "El NOMBRE es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        //Relación de UNO A MUCHOS
        [Required(ErrorMessage = "La PRIORIDAD es obligatoria")]
        [Display(Name = "Prioridad")]
        public int IdPrioridad { get; set; }
        [ForeignKey(nameof(IdPrioridad))]
        public Prioridad? Prioridad { get; set; }

        [Required(ErrorMessage = "El ESTADO es obligatorio")]
        [Display(Name = "Estado de la tarea")]
        public int IdEstadodeTarea { get; set; }
        [ForeignKey(nameof(IdEstadodeTarea))]
        [Display(Name = "Estado")]
        public EstadodeTarea? EstadodeTarea { get; set; }

        [Display(Name = "Fecha de Creacion")]
        public DateTime? FechaCreacion { get; set; }

        [Required(ErrorMessage = "La FECHA LIMITE es obligatoria")]
        [Display(Name = "Fecha Limite")]
        public DateTime FechaLimite { get; set; }

        //Relación MUCHOS A MUCHOS
        [Display(Name = "Asignacion")]
        public List<TareaEmpleado>? TareasEmpleados { get; set; }
    }
}
