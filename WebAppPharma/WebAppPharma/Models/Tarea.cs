﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class Tarea
    {
        [Key]
        public int IdTarea { get; set; }

        [Required(ErrorMessage = "El NOMBRE es obligatorio")]
        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "La DESCRIPCION es obligatoria")]
        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        //Relación de UNO A MUCHOS
        [Required(ErrorMessage = "La PRIORIDAD es obligatoria")]
        [Display(Name = "Prioridad")]
        public int IdPrioridad { get; set; }
        [ForeignKey(nameof(IdPrioridad))]
        public Prioridad? Prioridad { get; set; }

        [Required(ErrorMessage = "El ESTADO es obligatorio")]
        [Display(Name = "Estado")]
        public int IdEstadodeTarea { get; set; }
        [ForeignKey(nameof(IdEstadodeTarea))]
        public EstadodeTarea? EstadodeTarea { get; set; }

        [Required(ErrorMessage = "La FECHA CREACION es obligatoria")]
        [Display(Name = "Fecha de Creacion")]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "La FECHA LIMITE es obligatoria")]
        [Display(Name = "Fecha Limite")]
        public DateTime FechaLimite { get; set; }

        //Relación MUCHOS A MUCHOS
        [Required(ErrorMessage = "La ASIGNACION es obligatoria")]
        [Display(Name = "Asignacion")]
        public List<TareaEmpleado>? TareasEmpleados { get; set; }
    }
}