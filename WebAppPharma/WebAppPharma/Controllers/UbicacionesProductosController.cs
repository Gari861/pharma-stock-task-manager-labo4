using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPharma.Models;
using WebAppPharma.ViewModels;

namespace WebAppPharma.Controllers
{
    [Authorize(Roles = "Admin, Manager, Empleado")]
    public class UbicacionesProductosController : Controller
    {
        private readonly AppDBcontext _context;

        public UbicacionesProductosController(AppDBcontext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(UbicacionesProductosViewModel modelo, int pagina = 1)
        {
            int RegistrosPorPagina = 3;

            // Obtener los registros paginados de la base de datos, incluyendo el producto
            var registros = await _context.UbicacionesProductos
                .Include(u => u.Producto)  // Incluir la relación con Producto
                .Skip((pagina - 1) * RegistrosPorPagina)
                .Take(RegistrosPorPagina)
                .ToListAsync();

            // Asignar los registros a la propiedad Ubicaciones del modelo
            modelo.Ubicaciones = registros;

            // Configurar el paginador
            modelo.Paginador.PaginaActual = pagina;
            modelo.Paginador.RegistrosPorPagina = RegistrosPorPagina;
            modelo.Paginador.TotalRegistros = await _context.UbicacionesProductos.CountAsync();

            return View(modelo);
        }

        // GET: UbicacionesProductos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubicacionProducto = await _context.UbicacionesProductos
                .Include(u => u.Producto)
                .FirstOrDefaultAsync(m => m.IdUbicacion == id);
            if (ubicacionProducto == null)
            {
                return NotFound();
            }

            return View(ubicacionProducto);
        }

        [Authorize(Roles = "Admin")]
        // GET: UbicacionesProductos/Create
        public IActionResult Create()
        {
            // Obtener todos los IdProducto de los productos que ya tienen ubicación asignada
            var productosConUbicacion = _context.UbicacionesProductos
                .Select(up => up.IdProducto)
                .ToList();

            // Obtener todos los productos que no tienen ubicación asignada
            var productosSinUbicacion = _context.Productos
                .Where(p => !productosConUbicacion.Contains(p.IdProducto)) // Filtra los productos
                .ToList();

            // Crear el SelectList solo con productos sin ubicación
            ViewData["IdProducto"] = new SelectList(productosSinUbicacion, "IdProducto", "Nombre");

            return View();
        }


        // POST: UbicacionesProductos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUbicacion,Estante,Seccion,Pasillo,IdProducto")] UbicacionProducto ubicacionProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ubicacionProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", ubicacionProducto.IdProducto);
            return View(ubicacionProducto);
        }

        [Authorize(Roles = "Admin, Manager")]
        // GET: UbicacionesProductos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubicacionProducto = await _context.UbicacionesProductos.FindAsync(id);
            if (ubicacionProducto == null)
            {
                return NotFound();
            }

            // Obtener todos los IdProducto de los productos que ya tienen ubicación asignada
            var productosConUbicacion = _context.UbicacionesProductos
                .Select(up => up.IdProducto)
                .ToList();

            // Obtener todos los productos que no tienen ubicación asignada
            var productosSinUbicacion = _context.Productos
                .Where(p => !productosConUbicacion.Contains(p.IdProducto) || p.IdProducto == ubicacionProducto.IdProducto) // Filtra los productos
                .ToList();

            // Crear el SelectList solo con productos sin ubicación, incluyendo el producto actual
            ViewData["IdProducto"] = new SelectList(productosSinUbicacion, "IdProducto", "Nombre", ubicacionProducto.IdProducto);

            return View(ubicacionProducto);
        }

        [Authorize(Roles = "Admin, Manager")]
        // POST: UbicacionesProductos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUbicacion,Estante,Seccion,Pasillo,IdProducto")] UbicacionProducto ubicacionProducto)
        {
            if (id != ubicacionProducto.IdUbicacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ubicacionProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UbicacionProductoExists(ubicacionProducto.IdUbicacion))
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", ubicacionProducto.IdProducto);
            return View(ubicacionProducto);
        }

        [Authorize(Roles = "Admin")]
        // GET: UbicacionesProductos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ubicacionProducto = await _context.UbicacionesProductos
                .Include(u => u.Producto)
                .FirstOrDefaultAsync(m => m.IdUbicacion == id);
            if (ubicacionProducto == null)
            {
                return NotFound();
            }

            return View(ubicacionProducto);
        }

        [Authorize(Roles = "Admin")]
        // POST: UbicacionesProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ubicacionProducto = await _context.UbicacionesProductos.FindAsync(id);
            if (ubicacionProducto != null)
            {
                _context.UbicacionesProductos.Remove(ubicacionProducto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool UbicacionProductoExists(int id)
        {
            return _context.UbicacionesProductos.Any(e => e.IdUbicacion == id);
        }
    }
}
