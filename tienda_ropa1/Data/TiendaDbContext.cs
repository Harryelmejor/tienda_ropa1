using Microsoft.EntityFrameworkCore;
using tienda_ropa1.Models;

namespace tienda_ropa1.Data;

public class TiendaDbContext : DbContext
{
    public TiendaDbContext(DbContextOptions<TiendaDbContext> options) : base(options) { }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Prenda> Prendas => Set<Prenda>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetalleVentas => Set<DetalleVenta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.Property<int>("Id").HasColumnName("CategoriaID");
            entity.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Descripcion).HasMaxLength(255);
            entity.HasIndex(c => c.Nombre).IsUnique();
        });

        modelBuilder.Entity<Prenda>(entity =>
        {
            entity.ToTable("Productos");
            entity.Property<int>("Id").HasColumnName("ProductoID");
            entity.Property(p => p.Nombre).HasMaxLength(150).IsRequired();
            entity.Property(p => p.Descripcion).HasMaxLength(255);
            entity.Property(p => p.Precio).HasColumnType("decimal(10,2)");
            entity.Property(p => p.Talla).HasMaxLength(20);
            entity.Property(p => p.Color).HasMaxLength(50);
            entity.Property(p => p.CategoriaId).HasColumnName("CategoriaID");

            entity.HasOne(p => p.Categoria)
                .WithMany(c => c.Prendas)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Clientes");
            entity.Property<int>("Id").HasColumnName("ClienteID");
            entity.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Apellido).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Telefono).HasMaxLength(20);
            entity.Property(c => c.Email).HasMaxLength(150);
            entity.Property(c => c.Direccion).HasMaxLength(250);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("Empleados");
            entity.Property<int>("Id").HasColumnName("EmpleadoID");
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Apellido).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Telefono).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Cargo).HasMaxLength(100);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.ToTable("Ventas");
            entity.Property<int>("Id").HasColumnName("VentaID");
            entity.Property(v => v.ClienteId).HasColumnName("ClienteID");
            entity.Property(v => v.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(v => v.Total).HasColumnType("decimal(10,2)");

            entity.Ignore(v => v.NumeroFactura);

            entity.HasOne(v => v.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(v => v.Empleado)
                .WithMany(e => e.Ventas)
                .HasForeignKey(v => v.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.ToTable("DetalleVenta");
            entity.Property<int>("Id").HasColumnName("DetalleVentaID");
            entity.Property(d => d.VentaId).HasColumnName("VentaID");
            entity.Property(d => d.PrendaId).HasColumnName("ProductoID");
            entity.Property(d => d.PrecioUnitario).HasColumnType("decimal(10,2)");
            entity.Ignore(d => d.Subtotal);

            entity.HasOne(d => d.Venta)
                .WithMany(v => v.DetalleVentas)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Prenda)
                .WithMany(p => p.DetalleVentas)
                .HasForeignKey(d => d.PrendaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
