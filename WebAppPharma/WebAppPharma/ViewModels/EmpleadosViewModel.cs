using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppPharma.Models;
namespace WebAppPharma.ViewModels
{
    public class EmpleadosViewModel
    {
        // Lista de empleados que vamos a mostrar en la vista
        public List<Empleado>? Empleados { get; set; }

        // Lista de cargos para el dropdown en el filtro
        public SelectList? ListaCargos { get; set; }

        // Campos para los criterios de búsqueda
        public string? BusquedaNombre { get; set; }
        public string? BusquedaApellido { get; set; }
        public string? BusquedaDNI { get; set; }
        public int? BusquedaIdCargo { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
