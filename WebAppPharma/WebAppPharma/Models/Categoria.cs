using System.ComponentModel.DataAnnotations;

namespace WebAppPharma.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La CATEGORIA es obligatoria")]
        [Display(Name = "Categoría del Producto")]
        public string Tipo { get; set; }

        // Relación de MUCHOS A MUCHOS
        public List<ProductoCategoria>? ProductosCategorias { get; set; }
    }
}

