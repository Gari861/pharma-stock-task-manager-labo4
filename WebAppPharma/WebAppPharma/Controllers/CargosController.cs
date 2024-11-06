using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using WebAppPharma.Models;
using WebAppPharma.ViewModels;

namespace WebAppPharma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CargosController : Controller
    {
        private readonly AppDBcontext _context;
        
        public CargosController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: Cargos
        public async Task<IActionResult> Index(CargosViewModel modelo, int pagina = 1)
        {
            // Define que se mostrarán 3 registros por página.
            int RegistrosPorPagina = 3;

            // Aplica paginación a los datos obtenidos de entidad Cargo
            // La combinación permite cargar solo los datos necesarios para la página 
            // que seestá visualizando, para evitar cargar todos los registros a la vez.
            var registros = _context.Cargos
                .Skip((pagina - 1) * RegistrosPorPagina)
                //Skip se asegura de que solo veas los registros correspondientes a la página actual.
                .Take(RegistrosPorPagina);
            //Take limita la cantidad de registros obtenidos, evitando cargar todos los datos a la vez.

            /* Skip:
             * Si estás en página 1: (1 - 1) * 3 = 0 (no omite registros, muestra los primeros).
            Si estás en página 2: (2 - 1) * 3 = 3 (omite los primeros 3 registros y muestra los siguientes).
            Si estás en página 3: (3 - 1) * 3 = 6 (omite los primeros 6 registros y muestra los siguientes).
            */

            /*  Omitirá los registros de páginas anteriores con Skip.
                Seleccionará los registros de la página actual con Take.*/

            // Asigna los registros paginados al modelo
            modelo.Cargos = await registros.ToListAsync();

            // Configura la paginación: página actual, cantidad por página y total de registros.
            modelo.Paginador.PaginaActual = pagina; //1
            modelo.Paginador.RegistrosPorPagina = RegistrosPorPagina; //3
            modelo.Paginador.TotalRegistros = await _context.Cargos.CountAsync(); 

            return View(modelo);
        }

        // GET: Cargos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.IdCargo == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // GET: Cargos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cargos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCargo,Tipo")] Cargo cargo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cargo);
        }

        // GET: Cargos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }
            return View(cargo);
        }

        // POST: Cargos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCargo,Tipo")] Cargo cargo)
        {
            if (id != cargo.IdCargo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoExists(cargo.IdCargo))
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
            return View(cargo);
        }

        // GET: Cargos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargo = await _context.Cargos
                .FirstOrDefaultAsync(m => m.IdCargo == id);
            if (cargo == null)
            {
                return NotFound();
            }

            return View(cargo);
        }

        // POST: Cargos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo != null)
            {
                _context.Cargos.Remove(cargo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CargoExists(int id)
        {
            return _context.Cargos.Any(e => e.IdCargo == id);
        }
    }
}
