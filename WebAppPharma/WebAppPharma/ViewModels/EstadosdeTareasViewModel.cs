using WebAppPharma.Models;

namespace WebAppPharma.ViewModels
{
    public class EstadosdeTareasViewModel
    {
        public List<EstadodeTarea> EstadosdeTareas { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
