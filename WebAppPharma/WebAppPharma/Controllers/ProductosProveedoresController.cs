using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPharma.Models;

namespace WebAppPharma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductosProveedoresController : Controller
    {
        private readonly AppDBcontext _context;

        public ProductosProveedoresController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: ProductosProveedores
        public async Task<IActionResult> Index()
        {
            var appDBcontext = _context.ProductosProveedores.Include(p => p.Producto).Include(p => p.Proveedor);
            return View(await appDBcontext.ToListAsync());
        }

        // GET: ProductosProveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoProveedor = await _context.ProductosProveedores
                .Include(p => p.Producto)
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoProveedor == null)
            {
                return NotFound();
            }

            return View(productoProveedor);
        }

        // GET: ProductosProveedores/Create
        public IActionResult Create()
        {
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Apellido");
            return View();
        }

        // POST: ProductosProveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdProveedor")] ProductoProveedor productoProveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productoProveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", productoProveedor.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Apellido", productoProveedor.IdProveedor);
            return View(productoProveedor);
        }

        // GET: ProductosProveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoProveedor = await _context.ProductosProveedores.FindAsync(id);
            if (productoProveedor == null)
            {
                return NotFound();
            }
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", productoProveedor.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Apellido", productoProveedor.IdProveedor);
            return View(productoProveedor);
        }

        // POST: ProductosProveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdProveedor")] ProductoProveedor productoProveedor)
        {
            if (id != productoProveedor.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productoProveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoProveedorExists(productoProveedor.IdProducto))
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
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "Nombre", productoProveedor.IdProducto);
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Apellido", productoProveedor.IdProveedor);
            return View(productoProveedor);
        }

        // GET: ProductosProveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productoProveedor = await _context.ProductosProveedores
                .Include(p => p.Producto)
                .Include(p => p.Proveedor)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (productoProveedor == null)
            {
                return NotFound();
            }

            return View(productoProveedor);
        }

        // POST: ProductosProveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productoProveedor = await _context.ProductosProveedores.FindAsync(id);
            if (productoProveedor != null)
            {
                _context.ProductosProveedores.Remove(productoProveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoProveedorExists(int id)
        {
            return _context.ProductosProveedores.Any(e => e.IdProducto == id);
        }
    }
}
