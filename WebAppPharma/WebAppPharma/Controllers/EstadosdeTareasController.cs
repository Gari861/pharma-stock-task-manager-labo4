﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppPharma.Models;

namespace WebAppPharma.Controllers
{
    public class EstadosdeTareasController : Controller
    {
        private readonly AppDBcontext _context;

        public EstadosdeTareasController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: EstadosdeTareas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosdeTareas.ToListAsync());
        }

        // GET: EstadosdeTareas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodeTarea = await _context.EstadosdeTareas
                .FirstOrDefaultAsync(m => m.IdEstadodeTarea == id);
            if (estadodeTarea == null)
            {
                return NotFound();
            }

            return View(estadodeTarea);
        }

        // GET: EstadosdeTareas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadosdeTareas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstadodeTarea,Tipo")] EstadodeTarea estadodeTarea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadodeTarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadodeTarea);
        }

        // GET: EstadosdeTareas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodeTarea = await _context.EstadosdeTareas.FindAsync(id);
            if (estadodeTarea == null)
            {
                return NotFound();
            }
            return View(estadodeTarea);
        }

        // POST: EstadosdeTareas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstadodeTarea,Tipo")] EstadodeTarea estadodeTarea)
        {
            if (id != estadodeTarea.IdEstadodeTarea)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadodeTarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadodeTareaExists(estadodeTarea.IdEstadodeTarea))
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
            return View(estadodeTarea);
        }

        // GET: EstadosdeTareas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadodeTarea = await _context.EstadosdeTareas
                .FirstOrDefaultAsync(m => m.IdEstadodeTarea == id);
            if (estadodeTarea == null)
            {
                return NotFound();
            }

            return View(estadodeTarea);
        }

        // POST: EstadosdeTareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadodeTarea = await _context.EstadosdeTareas.FindAsync(id);
            if (estadodeTarea != null)
            {
                _context.EstadosdeTareas.Remove(estadodeTarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadodeTareaExists(int id)
        {
            return _context.EstadosdeTareas.Any(e => e.IdEstadodeTarea == id);
        }
    }
}