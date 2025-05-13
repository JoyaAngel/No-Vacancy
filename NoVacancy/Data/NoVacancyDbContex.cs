using NoVacancy.Models;
using Microsoft.EntityFrameworkCore;

namespace NoVacancy.Data;
public class NoVacancyDbContex : DbContext
{
    public NoVacancyDbContex(DbContextOptions<NoVacancyDbContex> options) : base(options) { }
    
    public DbSet<CarritoCabecera> Carritos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Color> Colores { get; set; }
    public DbSet<Imagen> Imagenes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Resenia> Resenias { get; set; }
    public DbSet<Talla> Tallas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de CarritoCabecera
        modelBuilder.Entity<CarritoCabecera>()
            .HasKey(c => new { c.idCarrito});

        modelBuilder.Entity<CarritoCabecera>()
            .HasOne(c => c.Cliente)
            .WithMany()
            .HasForeignKey(c => c.idCliente)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de CarritoLinea
        modelBuilder.Entity<CarritoLinea>()
            .HasKey(cl => new { cl.idCarrito, cl.idProducto });
        
        modelBuilder.Entity<CarritoLinea>()
            .HasOne(cl => cl.Carrito)
            .WithMany()
            .HasForeignKey(cl => cl.idCarrito)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CarritoLinea>()
            .HasOne(cl => cl.Producto)
            .WithMany()
            .HasForeignKey(cl => cl.idProducto)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de Cliente
        modelBuilder.Entity<Cliente>()
            .HasKey(c => c.idCliente);

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.correo)
            .IsUnique();

        // Configuración de Producto
        modelBuilder.Entity<Producto>()
            .HasKey(p => p.idProducto);

        modelBuilder.Entity<Producto>()
            .Property(p => p.precio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Talla)
            .WithMany()
            .HasForeignKey(p => p.idTalla)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria)
            .WithMany()
            .HasForeignKey(p => p.idCategoria)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Color)
            .WithMany()
            .HasForeignKey(p => p.idColor)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de Pedido
        modelBuilder.Entity<Pedido>()
            .HasKey(p => p.idPedido);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Carrito)
            .WithMany()
            .HasForeignKey(p => p.idCarrito)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de Detalle
        modelBuilder.Entity<Detalle>()
            .HasKey(d => new { d.idDetalle, d.idPedido});

        modelBuilder.Entity<Detalle>()
            .HasOne(d => d.Pedido)
            .WithMany()
            .HasForeignKey(d => d.idPedido)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuración de Imagen
        modelBuilder.Entity<Imagen>()
            .HasKey(i => i.idImagen);

        modelBuilder.Entity<Imagen>()
            .HasOne(i => i.Producto)
            .WithMany()
            .HasForeignKey(i => i.idProducto)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de Resenia
        modelBuilder.Entity<Resenia>()
            .HasKey(r => r.idResenia);

        modelBuilder.Entity<Resenia>()
            .HasOne(r => r.Producto)
            .WithMany()
            .HasForeignKey(r => r.idProducto)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de Talla
        modelBuilder.Entity<Talla>()
            .HasKey(t => t.idTalla);

        // Configuración de Categoria
        modelBuilder.Entity<Categoria>()
            .HasKey(c => c.idCategoria);

        // Configuración de Color
        modelBuilder.Entity<Color>()
            .HasKey(c => c.idColor);
    }

}
