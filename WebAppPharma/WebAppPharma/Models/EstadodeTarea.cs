using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class EstadodeTarea
    {
        [Key]
        public int IdEstadodeTarea { get; set; }

        [Required(ErrorMessage = "El ESTADO es obligatorio")]
        [Display(Name = "Estado de la Tarea")]
        public string Tipo { get; set; }

        //Relación de UNO A MUCHOS
        public List<Tarea>? Tareas { get; set; }
    }
}
