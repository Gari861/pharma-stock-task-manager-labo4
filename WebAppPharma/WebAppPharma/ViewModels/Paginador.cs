namespace WebAppPharma.ViewModels
{
    public class Paginador
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; } = 3;
        // Cantidad de registros a mostrar por página. Valor predeterminado de 3.

        public int TotalRegistros { get; set; }
        // Total de registros disponibles en la base de datos.

        public int TotalPagina => (int)Math.Ceiling((decimal)TotalRegistros / RegistrosPorPagina);
        // Calcula el total de páginas necesarias, redondeando hacia arriba.

        public Dictionary<string, string> ValoresQueryString { get; set; } = new Dictionary<string, string>();
        // Al cambiar de página que no se cambien los valores de los filtros.

    }
}
