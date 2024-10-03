using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPharma.Models;

namespace WebAppPharma.Controllers
{
    public class UbicacionesProductosController : Controller
    {
        private readonly AppDBcontext _context;

        public UbicacionesProductosController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: UbicacionesProductos
        public async Task<IActionResult> Index()
        {
            var appDBcontext = _context.UbicacionesProductos.Include(u => u.Producto);
            return View(await appDBcontext.ToListAsync());
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

        // GET: UbicacionesProductos/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            return View();
        }

        // POST: UbicacionesProductos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", ubicacionProducto.IdProducto);
            return View(ubicacionProducto);
        }

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

        private bool UbicacionProductoExists(int id)
        {
            return _context.UbicacionesProductos.Any(e => e.IdUbicacion == id);
        }
    }
}
