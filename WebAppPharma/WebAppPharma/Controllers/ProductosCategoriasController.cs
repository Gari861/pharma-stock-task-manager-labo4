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
    public class ProductosCategoriasController : Controller
    {
        private readonly AppDBcontext _context;

        public ProductosCategoriasController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: ProductosCategorias
        public async Task<IActionResult> Index()
        {
            var appDBcontext = _context.ProductosCategorias.Include(p => p.Categoria).Include(p => p.Producto);
            return View(await appDBcontext.ToListAsync());
        }

        // GET: ProductosCategorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCategoria = await _context.ProductosCategorias
                .Include(p => p.Categoria)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoCategoria == null)
            {
                return NotFound();
            }

            return View(productoCategoria);
        }

        // GET: ProductosCategorias/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Tipo");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            return View();
        }

        // POST: ProductosCategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdCategoria")] ProductoCategoria productoCategoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoCategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Tipo", productoCategoria.IdCategoria);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", productoCategoria.IdProducto);
            return View(productoCategoria);
        }

        // GET: ProductosCategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCategoria = await _context.ProductosCategorias.FindAsync(id);
            if (productoCategoria == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Tipo", productoCategoria.IdCategoria);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", productoCategoria.IdProducto);
            return View(productoCategoria);
        }

        // POST: ProductosCategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdCategoria")] ProductoCategoria productoCategoria)
        {
            if (id != productoCategoria.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoCategoriaExists(productoCategoria.IdProducto))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "Tipo", productoCategoria.IdCategoria);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", productoCategoria.IdProducto);
            return View(productoCategoria);
        }

        // GET: ProductosCategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoCategoria = await _context.ProductosCategorias
                .Include(p => p.Categoria)
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoCategoria == null)
            {
                return NotFound();
            }

            return View(productoCategoria);
        }

        // POST: ProductosCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoCategoria = await _context.ProductosCategorias.FindAsync(id);
            if (productoCategoria != null)
            {
                _context.ProductosCategorias.Remove(productoCategoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoCategoriaExists(int id)
        {
            return _context.ProductosCategorias.Any(e => e.IdProducto == id);
        }
    }
}
