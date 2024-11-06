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
    public class TareasEmpleadosController : Controller
    {
        private readonly AppDBcontext _context;

        public TareasEmpleadosController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: TareasEmpleados
        public async Task<IActionResult> Index()
        {
            var appDBcontext = _context.TareasEmpleados.Include(t => t.Empleado).Include(t => t.Tarea);
            return View(await appDBcontext.ToListAsync());
        }

        // GET: TareasEmpleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tareaEmpleado = await _context.TareasEmpleados
                .Include(t => t.Empleado)
                .Include(t => t.Tarea)
                .FirstOrDefaultAsync(m => m.IdTarea == id);
            if (tareaEmpleado == null)
            {
                return NotFound();
            }

            return View(tareaEmpleado);
        }

        // GET: TareasEmpleados/Create
        public IActionResult Create()
        {
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Apellido");
            ViewData["IdTarea"] = new SelectList(_context.Tareas, "IdTarea", "Descripcion");
            return View();
        }

        // POST: TareasEmpleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTarea,IdEmpleado")] TareaEmpleado tareaEmpleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tareaEmpleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Apellido", tareaEmpleado.IdEmpleado);
            ViewData["IdTarea"] = new SelectList(_context.Tareas, "IdTarea", "Descripcion", tareaEmpleado.IdTarea);
            return View(tareaEmpleado);
        }

        // GET: TareasEmpleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tareaEmpleado = await _context.TareasEmpleados.FindAsync(id);
            if (tareaEmpleado == null)
            {
                return NotFound();
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Apellido", tareaEmpleado.IdEmpleado);
            ViewData["IdTarea"] = new SelectList(_context.Tareas, "IdTarea", "Descripcion", tareaEmpleado.IdTarea);
            return View(tareaEmpleado);
        }

        // POST: TareasEmpleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTarea,IdEmpleado")] TareaEmpleado tareaEmpleado)
        {
            if (id != tareaEmpleado.IdTarea)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tareaEmpleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareaEmpleadoExists(tareaEmpleado.IdTarea))
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
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Apellido", tareaEmpleado.IdEmpleado);
            ViewData["IdTarea"] = new SelectList(_context.Tareas, "IdTarea", "Descripcion", tareaEmpleado.IdTarea);
            return View(tareaEmpleado);
        }

        // GET: TareasEmpleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tareaEmpleado = await _context.TareasEmpleados
                .Include(t => t.Empleado)
                .Include(t => t.Tarea)
                .FirstOrDefaultAsync(m => m.IdTarea == id);
            if (tareaEmpleado == null)
            {
                return NotFound();
            }

            return View(tareaEmpleado);
        }

        // POST: TareasEmpleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tareaEmpleado = await _context.TareasEmpleados.FindAsync(id);
            if (tareaEmpleado != null)
            {
                _context.TareasEmpleados.Remove(tareaEmpleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TareaEmpleadoExists(int id)
        {
            return _context.TareasEmpleados.Any(e => e.IdTarea == id);
        }
    }
}
