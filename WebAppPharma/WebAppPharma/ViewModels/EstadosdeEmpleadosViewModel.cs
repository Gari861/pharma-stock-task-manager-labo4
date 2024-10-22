using WebAppPharma.Models;

namespace WebAppPharma.ViewModels
{
    public class EstadosdeEmpleadosViewModel
    {
        public List<EstadodeEmpleado> EstadosdeEmpleados {  get; set; }
        public Paginador? Paginador { get; set; } = new Paginador();
    }
}
