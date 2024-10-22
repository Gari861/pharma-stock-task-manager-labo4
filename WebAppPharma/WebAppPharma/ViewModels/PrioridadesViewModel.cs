using WebAppPharma.Models;

namespace WebAppPharma.ViewModels
{
    public class PrioridadesViewModel
    {
        public List<Prioridad> Prioridades { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
