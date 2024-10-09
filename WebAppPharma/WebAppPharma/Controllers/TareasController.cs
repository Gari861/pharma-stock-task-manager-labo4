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
    public class TareasController : Controller
    {
        private readonly AppDBcontext _context;

        public TareasController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: Tareas
        public async Task<IActionResult> Index()
        {
            var appDBcontext = _context.Tareas.Include(t => t.EstadodeTarea).Include(t => t.Prioridad);
            return View(await appDBcontext.ToListAsync());
        }

        // GET: Tareas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.EstadodeTarea)
                .Include(t => t.Prioridad)
                .FirstOrDefaultAsync(m => m.IdTarea == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // GET: Tareas/Create
        public IActionResult Create()
        {
            var prioridades = _context.Prioridades.ToList();
            var estadosdetareas = _context.EstadosdeTareas.ToList();
            ViewData["IdEstadodeTarea"] = new SelectList(estadosdetareas, "IdEstadodeTarea", "Tipo", null)
                      .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewData["IdPrioridad"] = new SelectList(prioridades, "IdPrioridad", "Tipo", null)
                      .Prepend(new SelectListItem { Text = " ", Value = "" });
            // Verifica si no hay cargadas y pasa esa información a la vista
            ViewData["PrioridadesVacias"] = !prioridades.Any();
            ViewData["EstadosDeTareasVacios"] = !estadosdetareas.Any();
            return View();
        }


        // POST: Tareas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTarea,Nombre,Descripcion,IdPrioridad,IdEstadodeTarea,FechaCreacion,FechaLimite")] Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadodeTarea"] = new SelectList(_context.EstadosdeTareas, "IdEstadodeTarea", "Tipo", tarea.IdEstadodeTarea);
            ViewData["IdPrioridad"] = new SelectList(_context.Prioridades, "IdPrioridad", "Tipo", tarea.IdPrioridad);
            return View(tarea);
        }

        // GET: Tareas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }
            var prioridades = _context.Prioridades.ToList();
            var estadosdetareas = _context.EstadosdeTareas.ToList();

            ViewData["IdEstadodeTarea"] = new SelectList(_context.EstadosdeTareas, "IdEstadodeTarea", "Tipo", null)
                                  .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewData["IdPrioridad"] = new SelectList(_context.Prioridades, "IdPrioridad", "Tipo", null)
                                  .Prepend(new SelectListItem { Text = " ", Value = "" });

            // Verifica si no hay cargadas y pasa esa información a la vista
            ViewData["PrioridadesVacias"] = !prioridades.Any();
            ViewData["EstadosDeTareasVacios"] = !estadosdetareas.Any();

            return View(tarea);
        }

        // POST: Tareas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTarea,Nombre,Descripcion,IdPrioridad,IdEstadodeTarea,FechaCreacion,FechaLimite")] Tarea tarea)
        {
            if (id != tarea.IdTarea)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareaExists(tarea.IdTarea))
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
            ViewData["IdEstadodeTarea"] = new SelectList(_context.EstadosdeTareas, "IdEstadodeTarea", "Tipo", tarea.IdEstadodeTarea);
            ViewData["IdPrioridad"] = new SelectList(_context.Prioridades, "IdPrioridad", "Tipo", tarea.IdPrioridad);
            return View(tarea);
        }

        // GET: Tareas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.EstadodeTarea)
                .Include(t => t.Prioridad)
                .FirstOrDefaultAsync(m => m.IdTarea == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.IdTarea == id);
        }
    }
}
