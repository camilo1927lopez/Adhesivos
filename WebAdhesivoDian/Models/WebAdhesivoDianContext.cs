using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAdhesivoDian.Models;

namespace WebAdhesivoDian.Models
{
    public class WebAdhesivoDianContext : DbContext
    {


        public WebAdhesivoDianContext(DbContextOptions<WebAdhesivoDianContext> options) : base(options)
        {

        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cajero>().HasIndex("IdOficina", "Codigo").IsUnique();
            modelBuilder.Entity<Oficina>().HasIndex("idCliente", "Codigo").IsUnique();
            modelBuilder.Entity<Cliente>().HasIndex(t => t.Codigo).IsUnique();
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Oficina> Oficina { get; set; }
        public DbSet<Cajero> Cajero { get; set; }
        public DbSet<Encabezado> Encabezado { get; set; }
        public DbSet<DetalleEncabezado> DetalleEncabezado { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<WebAdhesivoDian.Models.Modulos> Modulos { get; set; }
        public DbSet<WebAdhesivoDian.Models.PermisoRolModulo> PermisoRolModulo { get; set; }
        public DbSet<WebAdhesivoDian.Models.EstadoPedido> EstadoPedido { get; set; }

    }
}
