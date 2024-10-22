using WebAppPharma.Models;

namespace WebAppPharma.ViewModels
{
    public class CargosViewModel
    {
        public List<Cargo> Cargos { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
