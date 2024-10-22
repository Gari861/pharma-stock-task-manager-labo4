using WebAppPharma.Models;

namespace WebAppPharma.ViewModels
{
    public class UbicacionesProductosViewModel
    {
        public List<UbicacionProducto> Ubicaciones {  get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
