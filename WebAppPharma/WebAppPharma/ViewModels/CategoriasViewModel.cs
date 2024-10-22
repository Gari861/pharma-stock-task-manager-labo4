using WebAppPharma.Models;

namespace WebAppPharma.ViewModels
{
    public class CategoriasViewModel
    {
        public List<Categoria> Categorias { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }
}
