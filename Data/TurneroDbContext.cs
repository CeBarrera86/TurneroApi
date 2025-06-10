using Microsoft.EntityFrameworkCore;
using TurneroApi.Models;
using TurneroApi.Data.Config;

namespace TurneroApi.Data;

public partial class TurneroDbContext : DbContext
{
    public TurneroDbContext() { }
    public TurneroDbContext(DbContextOptions<TurneroDbContext> options) : base(options) { }

    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Estado> Estados { get; set; }
    public virtual DbSet<Historial> Historiales { get; set; }
    public virtual DbSet<Mostrador> Mostradores { get; set; }
    public virtual DbSet<Puesto> Puestos { get; set; }
    public virtual DbSet<Rol> Roles { get; set; }
    public virtual DbSet<Sector> Sectores { get; set; }
    public virtual DbSet<Ticket> Tickets { get; set; }
    public virtual DbSet<Turno> Turnos { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.ApplyConfiguration(new ClienteConfig());
        modelBuilder.ApplyConfiguration(new EstadoConfig());
        modelBuilder.ApplyConfiguration(new HistorialConfig());
        modelBuilder.ApplyConfiguration(new MostradorConfig());
        modelBuilder.ApplyConfiguration(new PuestoConfig());
        modelBuilder.ApplyConfiguration(new RolConfig());
        modelBuilder.ApplyConfiguration(new SectorConfig());
        modelBuilder.ApplyConfiguration(new TicketConfig());
        modelBuilder.ApplyConfiguration(new TurnoConfig());
        modelBuilder.ApplyConfiguration(new UsuarioConfig());

        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}