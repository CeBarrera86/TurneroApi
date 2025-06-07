using Microsoft.EntityFrameworkCore;
using TurneroApi.Models.GeaPico;

namespace TurneroApi.Data;

public class GeaSeguridadDbContext : DbContext
{
    public GeaSeguridadDbContext(DbContextOptions<GeaSeguridadDbContext> options) : base(options) { }

    public DbSet<GeaUsuario> GeaUsuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Puedes agregar configuraciones específicas aquí si las columnas tienen nombres diferentes
        // o si hay relaciones. Para el ejemplo, con DataAnnotations en GeaUsuario ya es suficiente.
        // modelBuilder.Entity<GeaUsuario>().ToTable("Usuarios"); // Asegura el nombre de la tabla
        // modelBuilder.Entity<GeaUsuario>().HasKey(u => u.USU_CODIGO); // Si USU_CODIGO es la clave primaria
        base.OnModelCreating(modelBuilder);
    }
}