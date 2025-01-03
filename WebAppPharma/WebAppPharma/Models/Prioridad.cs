﻿using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class Prioridad
    {
        [Key]
        public int IdPrioridad { get; set; }

        [Required(ErrorMessage = "La PRIORIDAD es obligatoria")]
        [Display(Name = "Prioridad de la Tarea")]
        [StringLength(20, ErrorMessage = "La prioridad no puede exceder los 20 caracteres.")]
        public string Tipo { get; set; }

        // Relación de UNO A MUCHOS
        public List<Tarea>? Tareas { get; set; }
    }
}
