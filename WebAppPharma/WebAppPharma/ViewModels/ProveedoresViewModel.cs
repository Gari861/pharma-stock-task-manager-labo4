using WebAppPharma.Models;

namespace WebAppPharma.ViewModels
{
    public class ProveedoresViewModel
    {
        public List<Proveedor> Proveedores { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
