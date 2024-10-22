namespace WebAppPharma.ViewModels
{
    public class Paginador
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; } = 3;
        public int TotalRegistros { get; set; }
        public int TotalPagina => (int)Math.Ceiling((decimal)TotalRegistros / RegistrosPorPagina);
        public Dictionary<string, string> ValoresQueryString { get; set; } = new Dictionary<string, string>();
    }
}
