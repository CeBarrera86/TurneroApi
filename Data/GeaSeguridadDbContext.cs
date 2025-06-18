using Microsoft.EntityFrameworkCore;
using TurneroApi.Models.GeaPico;

namespace TurneroApi.Data;

public class GeaSeguridadDbContext : DbContext
{
    public GeaSeguridadDbContext(DbContextOptions<GeaSeguridadDbContext> options) : base(options) { }
    public DbSet<GeaSeguridad> GeaSeguridad { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GeaSeguridad>(entity =>
        {
            entity.ToTable("USUARIOS", schema: "dbo");
            entity.HasKey(u => u.USU_CODIGO);
        });

        base.OnModelCreating(modelBuilder);
    }
}