using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SpreadsheetLight;
using WebAppPharma.Models;
using WebAppPharma.ViewModels;

namespace WebAppPharma.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class EmpleadosController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly IWebHostEnvironment _env;
        public EmpleadosController(AppDBcontext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Empleados
        public async Task<IActionResult> Index(EmpleadosViewModel modelo)
        {
            // Verifica que el modelo no sea nulo; si lo es, crea una nueva instancia de EmpleadosViewModel.
            if (modelo == null)
            {
                modelo = new EmpleadosViewModel();
            }

            // Carga la lista de cargos para el dropdown en la vista.
            modelo.ListaCargos = new SelectList(await _context.Cargos.ToListAsync(), "IdCargo", "Tipo");

            // Crea una consulta inicial para obtener empleados, incluyendo sus relaciones con Cargo y Estado de Empleado.
            var empleadosQuery = _context.Empleados
                                         .Include(e => e.Cargo)
                                         .Include(e => e.EstadodeEmpleado)
                                         .AsQueryable();

            // Aplica filtros de búsqueda si los valores se proporcionaron en el modelo.
            if (!string.IsNullOrEmpty(modelo.BusquedaNombre))
            {
                empleadosQuery = empleadosQuery.Where(e => e.Nombre.Contains(modelo.BusquedaNombre));
            }

            if (!string.IsNullOrEmpty(modelo.BusquedaApellido))
            {
                empleadosQuery = empleadosQuery.Where(e => e.Apellido.Contains(modelo.BusquedaApellido));
            }

            if (!string.IsNullOrEmpty(modelo.BusquedaDNI))
            {
                empleadosQuery = empleadosQuery.Where(e => e.Dni.Contains(modelo.BusquedaDNI));
            }

            if (modelo.BusquedaIdCargo.HasValue)
            {
                empleadosQuery = empleadosQuery.Where(e => e.IdCargo == modelo.BusquedaIdCargo.Value);
            }

            // Ejecuta la consulta y asigna los resultados filtrados al modelo.
            modelo.Empleados = await empleadosQuery.ToListAsync();

            // Devuelve la vista con el modelo actualizado.
            return View(modelo);
        }

        // GET: /Productos/Import
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        // POST: /Productos/ImportarDatos
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ImportarDatos(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                var rutaDestino = Path.Combine(_env.WebRootPath, "importaciones");
                var extArch = Path.GetExtension(excelFile.FileName);
                if (extArch == ".xlsx" || extArch == ".xls")
                {
                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + extArch;
                    var rutaCompleta = Path.Combine(rutaDestino, archivoDestino);

                    using (var filestream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await excelFile.CopyToAsync(filestream);
                    }

                    SLDocument archXls = new SLDocument(rutaCompleta);
                    if (archXls != null)
                    {
                        List<Empleado> ListaEmpleados = new List<Empleado>();
                        int fila = 1;
                        while (!string.IsNullOrEmpty(archXls.GetCellValueAsString(fila, 1)))
                        {
                            try
                            {
                                Empleado empleado = new Empleado
                                {
                                    Nombre = archXls.GetCellValueAsString(fila, 1),
                                    Apellido = archXls.GetCellValueAsString(fila, 2),
                                    Dni = archXls.GetCellValueAsString(fila, 3),
                                    Telefono = archXls.GetCellValueAsString(fila, 4),
                                };

                                ListaEmpleados.Add(empleado);
                            }
                            catch (Exception ex)
                            {
                                ViewData["Error"] = $"Error al procesar la fila {fila}: {ex.Message}";
                                return View("Error");
                            }
                            fila++;
                        }

                        if (ListaEmpleados.Count > 0)
                        {
                            _context.Empleados.AddRange(ListaEmpleados);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            ViewData["Error"] = "No se encontraron datos válidos en el archivo Excel.";
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Empleados");
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Cargo)
                .Include(e => e.EstadodeEmpleado)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var cargos = _context.Cargos.ToList();
            var estadodeEmpleados = _context.EstadosdeEmpleados.ToList();
            ViewData["IdCargo"] = new SelectList(cargos, "IdCargo", "Tipo", null)
                                  .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewData["IdEstadodeEmpleado"] = new SelectList(estadodeEmpleados, "IdEstadodeEmpleado", "Tipo", null)
                                  .Prepend(new SelectListItem { Text = " ", Value = "" });

            // Verifica si no hay cargadas y pasa esa información a la vista
            ViewData["CargosVacios"] = !cargos.Any();
            ViewData["EstadosDeEmpleadosVacios"] = !estadodeEmpleados.Any();

            return View();
        }


        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,Nombre,Apellido,Dni,IdCargo,IdEstadodeEmpleado,Telefono,FechaNacimiento,Foto")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                // Manejo del archivo de foto
                var archivos = HttpContext.Request.Form.Files;

                // Verifica si hay archivos subidos
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];

                    // Verifica si el archivo subido tiene contenido
                    if (archivoFoto.Length > 0)
                    {
                        // Define la ruta de destino donde se guardará la foto
                        var rutaDestino = Path.Combine(_env.WebRootPath, "fotografias");
                        var extArch = Path.GetExtension(archivoFoto.FileName);

                        // Genera un nombre único para el archivo para evitar colisiones
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + extArch;

                        // Guarda el archivo en el servidor
                        using (var filestream = new FileStream(Path.Combine(rutaDestino, archivoDestino), FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            empleado.Foto = archivoDestino;
                        }
                    }
                }
                // Guarda los cambios en la base de datos
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCargo"] = new SelectList(_context.Cargos, "IdCargo", "Tipo", empleado.IdCargo);
            ViewData["IdEstadodeEmpleado"] = new SelectList(_context.EstadosdeEmpleados, "IdEstadodeEmpleado", "Tipo", empleado.IdEstadodeEmpleado);
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            var cargos = _context.Cargos.ToList();
            var estadodeEmpleados = _context.EstadosdeEmpleados.ToList();

            ViewData["IdCargo"] = new SelectList(_context.Cargos, "IdCargo", "Tipo", null)
                                  .Prepend(new SelectListItem { Text = " ", Value = "" });
            ViewData["IdEstadodeEmpleado"] = new SelectList(_context.EstadosdeEmpleados, "IdEstadodeEmpleado", "Tipo", null)
                                  .Prepend(new SelectListItem { Text = " ", Value = "" });
            // Verifica si no hay cargadas y pasa esa información a la vista
            ViewData["CargosVacios"] = !cargos.Any();
            ViewData["EstadosDeEmpleadosVacios"] = !estadodeEmpleados.Any();

            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpleado,Nombre,Apellido,Dni,IdCargo,IdEstadodeEmpleado,Telefono,FechaNacimiento,Foto")] Empleado empleado)
        {
            // Verifica si el ID del empleado proporcionado coincide con el ID del empleado que se está editando
            if (id != empleado.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Manejo de archivos (fotos)
                    var archivos = HttpContext.Request.Form.Files;

                    // Verifica si se ha subido algún archivo
                    if (archivos != null && archivos.Count > 0)
                    {
                        var archivoFoto = archivos[0];
                        // Verifica si el archivo subido tiene contenido
                        if (archivoFoto.Length > 0)
                        {
                            // Define la ruta de destino donde se guardará la nueva foto
                            var rutaDestino = Path.Combine(_env.WebRootPath, "fotografias");
                            var extArch = Path.GetExtension(archivoFoto.FileName);
                            var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + extArch;

                            // Crear el archivo en memoria
                            using (var filestream = new FileStream(Path.Combine(rutaDestino, archivoDestino), FileMode.Create))
                            {
                                archivoFoto.CopyTo(filestream);

                                // Si existe una foto anterior, se elimina
                                if (!string.IsNullOrEmpty(empleado.Foto))
                                {
                                    string fotoAnterior = Path.Combine(rutaDestino, empleado.Foto);
                                    if (System.IO.File.Exists(fotoAnterior))
                                    {
                                        System.IO.File.Delete(fotoAnterior);
                                    }
                                }

                                // Asigna la nueva foto al empleado
                                empleado.Foto = archivoDestino;
                            }
                        }
                    }

                    // Actualizar empleado en la base de datos
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.IdEmpleado))
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
            ViewData["IdCargo"] = new SelectList(_context.Cargos, "IdCargo", "Tipo", empleado.IdCargo);
            ViewData["IdEstadodeEmpleado"] = new SelectList(_context.EstadosdeEmpleados, "IdEstadodeEmpleado", "Tipo", empleado.IdEstadodeEmpleado);
            return View(empleado);
        }

        [Authorize(Roles = "Admin")]
        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Cargo)
                .Include(e => e.EstadodeEmpleado)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [Authorize(Roles = "Admin")]
        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.IdEmpleado == id);
        }
    }
}
