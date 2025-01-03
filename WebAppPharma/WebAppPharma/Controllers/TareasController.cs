﻿using System;
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
    public class TareasController : Controller
    {
        private readonly AppDBcontext _context;

        public TareasController(AppDBcontext context)
        {
            _context = context;
        }

        // GET: Tareas
        public async Task<IActionResult> Index(TareasViewModel modelo, int pagina = 1)
        {
            int RegistrosPorPagina = 3;

            // Obtener la lista de prioridades para el dropdown
            modelo.ListaPrioridades = new SelectList(await _context.Prioridades.ToListAsync(), "IdPrioridad", "Tipo");

            // Generar consulta inicial de tareas, incluyendo Propiedad y EstadodeTarea
            var tareasQuery = _context.Tareas
                                             .Include(e => e.Prioridad)
                                             .Include(e => e.EstadodeTarea)
                                             .Include(e => e.TareasEmpleados)
                                             .ThenInclude(te => te.Empleado)
                                             .AsQueryable();

            // Obtener la lista de empleados para el dropdown
            modelo.ListaEmpleados = await _context.Empleados
                                        .Select(e => new SelectListItem
                                        {
                                            Value = e.IdEmpleado.ToString(),
                                            Text = e.Nombre + " " + e.Apellido
                                        }).ToListAsync();

            // Aplicar filtros si se proporcionan
            if (!string.IsNullOrEmpty(modelo.BusquedaNombre))
            {
                tareasQuery = tareasQuery.Where(e => e.Nombre.Contains(modelo.BusquedaNombre));
            }
            if (modelo.BusquedaFechaLimite != null)
            {
                var fecha = modelo.BusquedaFechaLimite.Value.Date;
                tareasQuery = tareasQuery.Where(e => e.FechaLimite.Date == fecha);
            }
            if (modelo.BusquedaIdPrioridad.HasValue)
            {
                tareasQuery = tareasQuery.Where(e => e.IdPrioridad == modelo.BusquedaIdPrioridad.Value);
            }
            if (modelo.BusquedaEmpleadoId.HasValue)
            {
                tareasQuery = tareasQuery.Where(e => e.TareasEmpleados.Any(te => te.IdEmpleado == modelo.BusquedaEmpleadoId));
            }

            // Paginación: Obtener los registros paginados
            var registros = await tareasQuery
                                    .Skip((pagina - 1) * RegistrosPorPagina)
                                    .Take(RegistrosPorPagina)
                                    .ToListAsync();

            // Asignar los registros al modelo
            modelo.Tareas = registros;

            // Configurar el paginador
            modelo.Paginador.PaginaActual = pagina;
            modelo.Paginador.RegistrosPorPagina = RegistrosPorPagina;
            modelo.Paginador.TotalRegistros = await tareasQuery.CountAsync();

            // Pasar el modelo a la vista
            return View(modelo);
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
                .Include(t => t.TareasEmpleados)
                .ThenInclude(ta => ta.Empleado)
                .FirstOrDefaultAsync(m => m.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // GET: Tareas/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Obtiene las listas desde el contexto de datos y la almacena en una variable
            var prioridades = _context.Prioridades.ToList();
            var estadosdetareas = _context.EstadosdeTareas.ToList();

            // prepend es un SelectListItem vacío para permitir que el usuario seleccione un estado vacío.
            ViewData["IdEstadodeTarea"] = new SelectList(estadosdetareas, "IdEstadodeTarea", "Tipo", null)
                      .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewData["IdPrioridad"] = new SelectList(prioridades, "IdPrioridad", "Tipo", null)
                      .Prepend(new SelectListItem { Text = " ", Value = "" });

            // Verifica si no hay cargadas y pasa esa información a la vista
            ViewData["PrioridadesVacias"] = !prioridades.Any();
            ViewData["EstadosDeTareasVacios"] = !estadosdetareas.Any();

            // Carga la lista de empleados desde el contexto de datos, incluyendo su Cargo.
            // Los empleados son ordenados por tipo de cargo para que aparezcan en un orden lógico en el dropdown.
            ViewBag.Empleados = _context.Empleados
                .Include(e => e.Cargo)
                .OrderBy(e => e.Cargo.Tipo)
                .Select(e => new SelectListItem
                {
                    Value = e.IdEmpleado.ToString(),
                    Text = e.Nombre + " " + e.Apellido + " - " + e.Cargo.Tipo
                }).ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        // POST: Tareas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTarea,Nombre,Descripcion,IdPrioridad,IdEstadodeTarea,FechaCreacion,FechaLimite")] Tarea tarea, List<int> empleadosSeleccionados)
        // Lista de IDs de empleados seleccionados en el formulario
        {
            if (ModelState.IsValid)
            {
                // Agrega la nueva tarea al contexto
                _context.Add(tarea);
                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                /*Verificación de empleados seleccionados: Si hay empleados seleccionados, 
                el método crea una relación TareaEmpleado entre la tarea y cada empleado seleccionado. 
                */

                // Verifica si hay empleados seleccionados en la lista (puede estar vacía si no se seleccionaron)
                if (empleadosSeleccionados != null && empleadosSeleccionados.Count > 0)
                {
                    // Recorre cada empleado seleccionado en el formulario de creación
                    foreach (var idEmpleado in empleadosSeleccionados)
                    {
                        // Crea una nueva instancia de TareaEmpleado para relacionar el empleado con la tarea
                        var tareaEmpleado = new TareaEmpleado
                        {
                            IdTarea = tarea.IdTarea,
                            IdEmpleado = idEmpleado
                        };
                        // Agrega la relación tarea-empleado al contexto (marca para inserción en la base de datos)
                        _context.TareasEmpleados.Add(tareaEmpleado);
                    }
                    // Agrega la relación tarea-empleado al contexto (marca para inserción en la base de datos)
                    await _context.SaveChangesAsync();
                }
                // Redirige al índice después de crear la tarea y, si corresponde, asociarla con los empleados seleccionados
                return RedirectToAction(nameof(Index));
            }
            // Si el estado del modelo no es válido, carga datos necesarios y vuelve a mostrar el formulario

            ViewData["IdEstadodeTarea"] = new SelectList(_context.EstadosdeTareas, "IdEstadodeTarea", "Tipo", tarea.IdEstadodeTarea);
            ViewData["IdPrioridad"] = new SelectList(_context.Prioridades, "IdPrioridad", "Tipo", tarea.IdPrioridad);
            ViewBag.Empleados = _context.Empleados
                .Include(e => e.Cargo)
                .OrderBy(e => e.Cargo.Tipo)
                .Select(e => new SelectListItem
                {
                    Value = e.IdEmpleado.ToString(),
                    Text = e.Nombre + " " + e.Apellido + " - " + e.Cargo.Tipo
                }).ToList();
            return View(tarea);
        }

        // GET: Tareas/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(p => p.TareasEmpleados)
                .FirstOrDefaultAsync(m => m.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }

            var prioridades = _context.Prioridades.ToList();
            var estadosdetareas = _context.EstadosdeTareas.ToList();

            // Establece el valor seleccionado para Prioridad y Estado de Tarea
            ViewData["IdEstadodeTarea"] = new SelectList(_context.EstadosdeTareas, "IdEstadodeTarea", "Tipo", tarea.IdEstadodeTarea)
                .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewData["IdPrioridad"] = new SelectList(_context.Prioridades, "IdPrioridad", "Tipo", tarea.IdPrioridad)
                .Prepend(new SelectListItem { Text = " ", Value = "" });

            // Verifica si no hay cargadas y pasa esa información a la vista
            ViewData["PrioridadesVacias"] = !prioridades.Any();
            ViewData["EstadosDeTareasVacios"] = !estadosdetareas.Any();

            // Hacer que al traer el edit las marcadas sigan estando marcadas
            var empleadosSeleccionados = tarea.TareasEmpleados.Select(pc => pc.IdEmpleado).ToList();

            ViewBag.Empleados = _context.Empleados
                            .Include(e => e.Cargo)
                            .OrderBy(e => e.Cargo.Tipo)
                            .Select(e => new SelectListItem
                            {
                                Value = e.IdEmpleado.ToString(),
                                Text = e.Nombre + " " + e.Apellido + " - " + e.Cargo.Tipo,
                                Selected = empleadosSeleccionados.Contains(e.IdEmpleado)
                            }).ToList();

            return View(tarea);
        }

        // POST: Tareas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTarea,Nombre,Descripcion,IdPrioridad,IdEstadodeTarea,FechaCreacion,FechaLimite")] Tarea tarea, List<int> empleadosSeleccionados)
        {
            if (id != tarea.IdTarea)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Indica al contexto que el objeto 'tarea' ha sido modificado
                    _context.Update(tarea);
                    await _context.SaveChangesAsync();

                    // Eliminar relaciones antiguas de TareasEmpleados
                    var empleadosAntiguos = _context.TareasEmpleados.Where(pc => pc.IdTarea == tarea.IdTarea);
                    _context.TareasEmpleados.RemoveRange(empleadosAntiguos);
                    await _context.SaveChangesAsync();

                    // Agregar nuevas relaciones de TareasEmpleados
                    if (empleadosSeleccionados != null && empleadosSeleccionados.Count > 0)
                    {
                        foreach (var idEmpleado in empleadosSeleccionados)
                        {
                            var tareaEmpleado = new TareaEmpleado
                            {
                                IdTarea = tarea.IdTarea,
                                IdEmpleado = idEmpleado
                            };
                            _context.TareasEmpleados.Add(tareaEmpleado);
                        }
                        await _context.SaveChangesAsync();
                    }
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

            // Si ModelState no es válido, repopular las listas desplegables
            ViewData["IdEstadodeTarea"] = new SelectList(_context.EstadosdeTareas, "IdEstadodeTarea", "Tipo", tarea.IdEstadodeTarea)
                .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewData["IdPrioridad"] = new SelectList(_context.Prioridades, "IdPrioridad", "Tipo", tarea.IdPrioridad)
                .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewBag.Empleados = _context.Empleados
                            .Include(e => e.Cargo)
                            .OrderBy(e => e.Cargo.Tipo)
                            .Select(e => new SelectListItem
                            {
                                Value = e.IdEmpleado.ToString(),
                                Text = e.Nombre + " " + e.Apellido + " - " + e.Cargo.Tipo,
                                Selected = empleadosSeleccionados.Contains(e.IdEmpleado)
                            }).ToList();
            return View(tarea);
        }


        // GET: Tareas/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.EstadodeTarea)
                .Include(t => t.Prioridad)
                .Include(l => l.TareasEmpleados).ThenInclude(la => la.Empleado)
                .FirstOrDefaultAsync(m => m.IdTarea == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                var tareaEmpleados = _context.TareasEmpleados.Where(te => te.IdTarea == id).ToList();
                if (tareaEmpleados.Any())
                {
                    _context.TareasEmpleados.RemoveRange(tareaEmpleados);
                }
                _context.Tareas.Remove(tarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.IdTarea == id);
        }
    }
}
