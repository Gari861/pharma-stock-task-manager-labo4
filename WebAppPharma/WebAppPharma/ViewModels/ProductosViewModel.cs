using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppPharma.Models;
namespace WebAppPharma.ViewModels
{
    public class ProductosViewModel
    {
        // Lista de productos que vamos a mostrar en la vista
        public List<Producto>? Productos { get; set; }

        // Campos para los criterios de búsqueda
        public string? BusquedaCod { get; set; }
        public string? BusquedaNombre { get; set; }
        public DateTime? BusquedaFechaVencimiento { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
