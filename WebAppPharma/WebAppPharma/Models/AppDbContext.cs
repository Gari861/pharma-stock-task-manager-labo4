using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppLibros.Models;

namespace WebAppPharma.Models
{
    public class AppDBcontext : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=Labo4; Trusted_Connection=True; MultipleActiveResultSets=True");
        //}

        // inyección de dependencia SQL
        public AppDBcontext(DbContextOptions<AppDBcontext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación UNO A UNO 
            modelBuilder.Entity<Producto>()
                .HasOne(l => l.UbicacionProducto)
                .WithOne(u => u.Producto)
                .HasForeignKey<UbicacionBiblioteca>(u => u.IdProducto);

            // Relación UNO A MUCHOS
            modelBuilder.Entity<Tarea>()
                .HasOne(l => l.Prioridad)
                .WithMany(g => g.Tarea)
                .HasForeignKey(l => l.IdPrioridad);

            modelBuilder.Entity<Tarea>()
                .HasOne(l => l.EstadodeTarea)
                .WithMany(g => g.Tarea)
                .HasForeignKey(l => l.IdEstadodeTarea);

            modelBuilder.Entity<Empleado>()
                .HasOne(l => l.Cargo)
                .WithMany(g => g.Empleado)
                .HasForeignKey(l => l.IdCargo);

            // Relación MUCHOS A MUCHOS
            modelBuilder.Entity<ProductoCategoria>()
                .HasKey(pc => new { pc.IdProducto, pc.IdCategoria });

            modelBuilder.Entity<ProductoCategoria>()
                .HasOne(pc => pc.Producto)
                .WithMany(p => p.ProductosCategorias)
                .HasForeignKey(pc => pc.IdProducto);

            modelBuilder.Entity<ProductoCategoria>()
                .HasOne(pc => pc.Categoria)
                .WithMany(c => c.ProductosCategorias)
                .HasForeignKey(pc => pc.IdCategoria);

            modelBuilder.Entity<ProductoProveedor>()
                .HasKey(pp => new { pp.IdProducto, pp.IdProveedor });

            modelBuilder.Entity<ProductoProveedor>()
                .HasOne(pp => pp.Producto)
                .WithMany(p => p.ProductosProveedores)
                .HasForeignKey(pp => pp.IdProducto);

            modelBuilder.Entity<ProductoProveedor>()
                .HasOne(pp => pp.Proveedor)
                .WithMany(pv => pv.ProductosProveedores)
                .HasForeignKey(pp => pp.IdProveedor);

            //asignacion


        }

        //Tablas
        public DbSet<Producto> Productos { get; set; }
        public DbSet<UbicacionProducto> UbicacionesProductos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<ProductoCategoria> ProductosCategorias { get; set; }
        public DbSet<ProductoProveedor> ProductosProveedores { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<EstadodeTarea> EstadosdeTareas { get; set; }
        public DbSet<EstadodeTarea> EstadosdeTareas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<EstadodeEmpleado> EstadosdeEmpleados { get; set; }
    }
}


