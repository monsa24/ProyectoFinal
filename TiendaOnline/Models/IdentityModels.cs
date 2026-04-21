using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TiendaOnline.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        [StringLength(80)]
        public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que authenticationType debe coincidir con el valor definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar reclamaciones de usuario personalizadas aquí
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ProductoModels> Productos { get; set; }
        public DbSet<CompraModels> Compras { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleOrden> DetallesOrden { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompraModels>()
                .HasRequired(m => m.Producto)
                .WithMany(c => c.Compras)
                .HasForeignKey(m => m.ProductoId)
                .WillCascadeOnDelete(true);
        }
    }

    // ── ITEM DEL CARRITO (solo en Session, no en BD) ─────────────────────
    public class ItemCarrito
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get { return Precio * Cantidad; } }
    }

    // ── ORDEN ─────────────────────────────────────────────────────────────
    public class Orden
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public string Direccion { get; set; }
        public string MetodoPago { get; set; }
        public virtual ICollection<DetalleOrden> Detalles { get; set; }
    }

    // ── DETALLE DE ORDEN ──────────────────────────────────────────────────
    public class DetalleOrden
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get { return Precio * Cantidad; } }
        public virtual Orden Orden { get; set; }
    }
}