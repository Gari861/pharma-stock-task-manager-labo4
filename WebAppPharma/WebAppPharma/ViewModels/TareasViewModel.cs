using Microsoft.AspNetCore.Mvc.Rendering;
using WebAppPharma.Models;
namespace WebAppPharma.ViewModels
{
    public class TareasViewModel
    {
        public List<Tarea>? Tareas { get; set; }
        public string? BusquedaNombre { get; set; }
        public DateTime? BusquedaFechaLimite { get; set; }
        public int? BusquedaIdPrioridad { get; set; }

        // Filtro para prioridades asignadas a tareas
        public SelectList? ListaPrioridades { get; set; }

        // Filtro para empleados asignados a tareas
        public int? BusquedaEmpleadoId { get; set; }
        public IEnumerable<SelectListItem>? ListaEmpleados { get; set; }
        public Paginador? Paginador { get; set; } = new Paginador();
    }
}
