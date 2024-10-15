using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpreadsheetLight;
using WebAppPharma.Models;
using WebAppPharma.ViewModels;

namespace WebAppPharma.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly IWebHostEnvironment _env;

        // inyección de dependencia SQL
        public ProductosController(AppDBcontext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Productos
        public async Task<IActionResult> Index(ProductosViewModel modelo)
        {
            //var appDBcontext = _context.Productos.Include(l => l.ProductosCategorias).ThenInclude(la => la.Categoria).Include(l => l.ProductosProveedores).ThenInclude(la => la.Proveedor);

            var productosQuery = _context.Productos
                             .Include(e => e.ProductosCategorias)
                             .ThenInclude(la => la.Categoria)
                             .Include(e => e.ProductosProveedores)
                             .ThenInclude(la => la.Proveedor)
                             .AsQueryable();
            if (!string.IsNullOrEmpty(modelo.BusquedaCod))
            {
                productosQuery = productosQuery.Where(e => e.CodigoProducto.Contains(modelo.BusquedaCod));
            }
            if (!string.IsNullOrEmpty(modelo.BusquedaNombre))
            {
                productosQuery = productosQuery.Where(e => e.Nombre.Contains(modelo.BusquedaNombre));
            }
            if (modelo.BusquedaFechaVencimiento != null)
            {
                var fecha = modelo.BusquedaFechaVencimiento.Value.Date;
                var fechaFin = fecha.AddDays(1).Date; // El día siguiente para definir el final del rango
                productosQuery = productosQuery.Where(e => e.FechaVencimiento >= fecha && e.FechaVencimiento < fechaFin);
            }

            // Ejecutar la consulta y asignar los resultados al modelo
            modelo.Productos = await productosQuery.ToListAsync();

            //return View(await appDBcontext.ToListAsync());
            return View(modelo);
        }

        // GET: /Productos/Import
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        // POST: /Productos/ImportarDatosEsenciales
        [HttpPost]
        public async Task<IActionResult> ImportarDatosEsenciales(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                var rutaDestino = Path.Combine(_env.WebRootPath, "importaciones");
                var extArch = Path.GetExtension(excelFile.FileName);
                if (extArch == ".xlsx" || extArch == ".xls")
                {
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + extArch;
                    var rutaCompleta = Path.Combine(rutaDestino, archivoDestino);

                    using (var filestream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await excelFile.CopyToAsync(filestream);
                    }

                    SLDocument archXls = new SLDocument(rutaCompleta);
                    if (archXls != null)
                    {
                        List<Producto> ListaProductos = new List<Producto>();
                        int fila = 1;
                        while (!string.IsNullOrEmpty(archXls.GetCellValueAsString(fila, 1)))
                        {
                            try
                            {
                                Producto producto = new Producto
                                {
                                    Nombre = archXls.GetCellValueAsString(fila, 1),
                                    CantSock = int.Parse(archXls.GetCellValueAsString(fila, 2))
                                };

                                ListaProductos.Add(producto);
                            }
                            catch (Exception ex)
                            {
                                ViewData["Error"] = $"Error al procesar la fila {fila}: {ex.Message}";
                                return View("Error");
                            }
                            fila++;
                        }

                        if (ListaProductos.Count > 0)
                        {
                            _context.Productos.AddRange(ListaProductos);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            ViewData["Error"] = "No se encontraron datos válidos en el archivo Excel.";
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Productos");
        }

        // POST: /Productos/ImportarDatosAdicionales
        [HttpPost]
        public async Task<IActionResult> ImportarDatosAdicionales(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                var rutaDestino = Path.Combine(_env.WebRootPath, "importaciones");
                var extArch = Path.GetExtension(excelFile.FileName);
                if (extArch == ".xlsx" || extArch == ".xls")
                {
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + extArch;
                    var rutaCompleta = Path.Combine(rutaDestino, archivoDestino);

                    using (var filestream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await excelFile.CopyToAsync(filestream);
                    }

                    SLDocument archXls = new SLDocument(rutaCompleta);
                    if (archXls != null)
                    {
                        List<Producto> ListaProductos = new List<Producto>();

                        int fila = 1;
                        while (!string.IsNullOrEmpty(archXls.GetCellValueAsString(fila, 2))) // Nombre del producto está en la columna 2
                        {
                            try
                            {
                                Producto producto = new Producto
                                {
                                    CodigoProducto = archXls.GetCellValueAsString(fila, 1),  // Opcional
                                    Nombre = archXls.GetCellValueAsString(fila, 2),         // Nombre
                                    Precio = decimal.Parse(archXls.GetCellValueAsString(fila, 3)),  // Opcional
                                    CantSock = int.Parse(archXls.GetCellValueAsString(fila, 4))  // Stock
                                };
                                ListaProductos.Add(producto);
                            }
                            catch (Exception ex)
                            {
                                ViewData["Error"] = $"Error al procesar la fila {fila}: {ex.Message}";
                                return View("Error");
                            }
                            fila++;
                        }

                        if (ListaProductos.Count > 0)
                        {
                            _context.Productos.AddRange(ListaProductos);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            ViewData["Error"] = "No se encontraron datos válidos en el archivo Excel.";
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Productos");
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(l => l.ProductosCategorias).ThenInclude(la => la.Categoria)
                .Include(l => l.ProductosProveedores).ThenInclude(la => la.Proveedor)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewBag.Categorias = new MultiSelectList(_context.Categorias, "IdCategoria", "Tipo");
            ViewBag.Proveedores = new MultiSelectList(_context.Proveedores, "IdProveedor", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,CodigoProducto,Nombre,Precio,CantSock,Foto,FechaIngreso,FechaVencimiento")] Producto producto, List<int> categoriasSeleccionadas, List<int> proveedoresSeleccionados)
        {
            // Normalizar el precio para que acepte tanto comas como puntos
            if (!string.IsNullOrEmpty(producto.Precio.ToString()))
            {
                // Reemplaza la coma con el punto en caso de que el usuario la haya ingresado
                producto.Precio = Convert.ToDecimal(producto.Precio.ToString().Replace(',', '.'));
            }

            if (ModelState.IsValid)
            {
                // Manejo del archivo de foto
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        var rutaDestino = Path.Combine(_env.WebRootPath, "fotografias");
                        var extArch = Path.GetExtension(archivoFoto.FileName);
                        // Generar un nombre único para el archivo
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + extArch;

                        // Guardar el archivo en memoria
                        using (var filestream = new FileStream(Path.Combine(rutaDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            producto.Foto = archivoDestino;
                        }
                    }
                }
                // Guardar el producto en la base de datos
                _context.Add(producto);
                await _context.SaveChangesAsync();

                // Relacionar las categorias seleccionadas con el producto
                if (categoriasSeleccionadas != null && categoriasSeleccionadas.Count > 0)
                {
                    foreach (var idCategoria in categoriasSeleccionadas)
                    {
                        var productoCategoria = new ProductoCategoria
                        {
                            IdProducto = producto.IdProducto,
                            IdCategoria = idCategoria
                        };
                        _context.ProductosCategorias.Add(productoCategoria);
                    }
                    await _context.SaveChangesAsync();
                }

                // Relacionar las proveedores seleccionados con el producto
                if (proveedoresSeleccionados != null && proveedoresSeleccionados.Count > 0)
                {
                    foreach (var idProveedor in proveedoresSeleccionados)
                    {
                        var productoProveedor = new ProductoProveedor
                        {
                            IdProducto = producto.IdProducto,
                            IdProveedor = idProveedor
                        };
                        _context.ProductosProveedores.Add(productoProveedor);
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            // Volver a cargar la lista
            ViewBag.Categorias = new MultiSelectList(_context.Categorias, "IdCategoria", "Tipo");
            ViewBag.Proveedores = new MultiSelectList(_context.Proveedores, "IdProveedor", "Nombre");
            return View(producto);
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.ProductosCategorias)
                .Include(p => p.ProductosProveedores)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            // Obtener categorías seleccionadas
            var categoriasSeleccionadas = producto.ProductosCategorias.Select(pc => pc.IdCategoria).ToList();

            // Obtener proveedores seleccionados
            var proveedoresSeleccionados = producto.ProductosProveedores.Select(pp => pp.IdProveedor).ToList();

            ViewBag.Categorias = new MultiSelectList(_context.Categorias, "IdCategoria", "Tipo", categoriasSeleccionadas);
            ViewBag.Proveedores = new MultiSelectList(_context.Proveedores, "IdProveedor", "Nombre", proveedoresSeleccionados);

            return View(producto);
        }


        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,CodigoProducto,Nombre,Precio,CantSock,Foto,FechaIngreso,FechaVencimiento")] Producto producto, List<int> categoriasSeleccionadas, List<int> proveedoresSeleccionados)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            // Normalizar el precio para que acepte tanto comas como puntos
            if (!string.IsNullOrEmpty(producto.Precio.ToString()))
            {
                producto.Precio = Convert.ToDecimal(producto.Precio.ToString().Replace(',', '.'));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Manejo de archivos (fotos)
                    var archivos = HttpContext.Request.Form.Files;
                    if (archivos != null && archivos.Count > 0)
                    {
                        var archivoFoto = archivos[0];
                        if (archivoFoto.Length > 0)
                        {
                            var rutaDestino = Path.Combine(_env.WebRootPath, "fotografias");
                            var extArch = Path.GetExtension(archivoFoto.FileName);
                            var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + extArch;

                            // Crear el archivo en memoria
                            using (var filestream = new FileStream(Path.Combine(rutaDestino, archivoDestino), FileMode.Create))
                            {
                                archivoFoto.CopyTo(filestream);

                                // Si existe una foto anterior, se elimina
                                if (!string.IsNullOrEmpty(producto.Foto))
                                {
                                    string fotoAnterior = Path.Combine(rutaDestino, producto.Foto);
                                    if (System.IO.File.Exists(fotoAnterior))
                                    {
                                        System.IO.File.Delete(fotoAnterior);
                                    }
                                }

                                // Asignar la nueva foto
                                producto.Foto = archivoDestino;
                            }
                        }
                    }
                    _context.Update(producto);
                    await _context.SaveChangesAsync();

                    // Eliminar relaciones antiguas en ProductoCategoria
                    var categoriasAntiguas = _context.ProductosCategorias.Where(la => la.IdProducto == producto.IdProducto);
                    _context.ProductosCategorias.RemoveRange(categoriasAntiguas);
                    await _context.SaveChangesAsync();

                    if (categoriasSeleccionadas != null && categoriasSeleccionadas.Count > 0)
                    {
                        foreach (var idCategoria in categoriasSeleccionadas)
                        {
                            var productoCategoria = new ProductoCategoria
                            {
                                IdProducto = producto.IdProducto,
                                IdCategoria = idCategoria
                            };
                            _context.ProductosCategorias.Add(productoCategoria);
                        }
                        await _context.SaveChangesAsync();
                    }

                    // Eliminar relaciones antiguas en ProductoProveedor
                    var proveedoresAntiguos = _context.ProductosProveedores.Where(lc => lc.IdProducto == producto.IdProducto);
                    _context.ProductosProveedores.RemoveRange(proveedoresAntiguos);
                    await _context.SaveChangesAsync();

                    if (proveedoresSeleccionados != null && proveedoresSeleccionados.Count > 0)
                    {
                        foreach (var idProveedor in proveedoresSeleccionados)
                        {
                            var productoProveedor = new ProductoProveedor
                            {
                                IdProducto = producto.IdProducto,
                                IdProveedor = idProveedor
                            };
                            _context.ProductosProveedores.Add(productoProveedor);
                        }
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            // Volver a cargar la lista
            ViewBag.Categorias = new MultiSelectList(_context.Categorias, "IdCategoria", "Tipo");
            ViewBag.Proveedores = new MultiSelectList(_context.Proveedores, "IdProveedor", "Nombre");
            return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(l => l.ProductosCategorias).ThenInclude(la => la.Categoria)
                .Include(l => l.ProductosProveedores).ThenInclude(la => la.Proveedor)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
