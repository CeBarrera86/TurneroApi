using Microsoft.EntityFrameworkCore;
using TurneroApi.Models.GeaPico;

namespace TurneroApi.Data;

public class GeaSeguridadDbContext : DbContext
{
    public GeaSeguridadDbContext(DbContextOptions<GeaSeguridadDbContext> options) : base(options) { }

    // Existing DbSet for authentication
    public DbSet<GeaSeguridad> GeaSeguridad { get; set; } = null!;

    // REMOVED: DbSet<GeaCorpicoDNI> GeaCorpicoClienteDni { get; set; }
    // REMOVED: DbSet<GeaCorpicoCliente> GeaCorpicoClientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuration for GeaUsuario (assuming it maps to GeaSeguridad_Corpico.dbo.Usuarios)
        modelBuilder.Entity<GeaSeguridad>().ToTable("Usuarios", schema: "dbo");

        // REMOVED: All configurations related to GeaCorpicoDNI and GeaCorpicoCliente

        base.OnModelCreating(modelBuilder);
    }
}