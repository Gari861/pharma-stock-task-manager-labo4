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
    [Authorize]
    public class EstadosdeEmpleadosController : Controller
    {
        private readonly AppDBcontext _context;

        public EstadosdeEmpleadosController(AppDBcontext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: EstadosdeEmpleados
        public async Task<IActionResult> Index(EstadosdeEmpleadosViewModel modelo, int pagina = 1)
        {
            int RegistrosPorPagina = 3;

            var registros = _context.EstadosdeEmpleados
                .Skip((pagina - 1) * RegistrosPorPagina)
                                .Take(RegistrosPorPagina);

            // Asignar los registros paginados al modelo
            modelo.EstadosdeEmpleados = await registros.ToListAsync();
            modelo.Paginador.PaginaActual = pagina;
            modelo.Paginador.RegistrosPorPagina = RegistrosPorPagina;
            modelo.Paginador.TotalRegistros = await _context.EstadosdeEmpleados.CountAsync();

            return View(modelo);
        }

        [AllowAnonymous]
        // GET: EstadosdeEmpleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodeEmpleado = await _context.EstadosdeEmpleados
                .FirstOrDefaultAsync(m => m.IdEstadodeEmpleado == id);
            if (estadodeEmpleado == null)
            {
                return NotFound();
            }

            return View(estadodeEmpleado);
        }

        // GET: EstadosdeEmpleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadosdeEmpleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstadodeEmpleado,Tipo")] EstadodeEmpleado estadodeEmpleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadodeEmpleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadodeEmpleado);
        }

        // GET: EstadosdeEmpleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodeEmpleado = await _context.EstadosdeEmpleados.FindAsync(id);
            if (estadodeEmpleado == null)
            {
                return NotFound();
            }
            return View(estadodeEmpleado);
        }

        // POST: EstadosdeEmpleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstadodeEmpleado,Tipo")] EstadodeEmpleado estadodeEmpleado)
        {
            if (id != estadodeEmpleado.IdEstadodeEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadodeEmpleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadodeEmpleadoExists(estadodeEmpleado.IdEstadodeEmpleado))
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
            return View(estadodeEmpleado);
        }

        // GET: EstadosdeEmpleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodeEmpleado = await _context.EstadosdeEmpleados
                .FirstOrDefaultAsync(m => m.IdEstadodeEmpleado == id);
            if (estadodeEmpleado == null)
            {
                return NotFound();
            }

            return View(estadodeEmpleado);
        }

        // POST: EstadosdeEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadodeEmpleado = await _context.EstadosdeEmpleados.FindAsync(id);
            if (estadodeEmpleado != null)
            {
                _context.EstadosdeEmpleados.Remove(estadodeEmpleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadodeEmpleadoExists(int id)
        {
            return _context.EstadosdeEmpleados.Any(e => e.IdEstadodeEmpleado == id);
        }
    }
}
