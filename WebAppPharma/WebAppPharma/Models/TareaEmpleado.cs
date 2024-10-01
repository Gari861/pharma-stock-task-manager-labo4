using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppPharma.Models
{
    public class TareaEmpleado
    {
        //Relación de MUCHOS A MUCHOS
        public int IdTarea { get; set; }
        [ForeignKey(nameof(IdTarea))]
        public Tarea? Tarea { get; set; }

        public int IdEmpleado { get; set; }
        [ForeignKey(nameof(IdEmpleado))]
        public Empleado? Empleado { get; set; }
    }
}
