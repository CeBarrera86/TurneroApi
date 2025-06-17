using Microsoft.EntityFrameworkCore;
using TurneroApi.Models.GeaPico; // Or the appropriate namespace for your GeaCorpico models

namespace TurneroApi.Data;

public class GeaCorpicoDbContext : DbContext
{
    public GeaCorpicoDbContext(DbContextOptions<GeaCorpicoDbContext> options) : base(options) { }

    public DbSet<GeaCorpicoDNI> GeaCorpicoClienteDocumentos { get; set; } = null!; // Keep this as "Documentos" for clarity
    public DbSet<GeaCorpicoCliente> GeaCorpicoClientes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuration for GeaCorpicoDNI (CLIENTE_DOCUMENTO)
        modelBuilder.Entity<GeaCorpicoDNI>()
            .ToTable("CLIENTE_DOCUMENTO", schema: "dbo");

        // Configure the relationship: GeaCorpicoDNI (Many) has one GeaCorpicoCliente (One)
        // This is a "Many-to-One" relationship. The foreign key (CLD_CLIENTE) is in GeaCorpicoDNI.
        modelBuilder.Entity<GeaCorpicoDNI>()
            .HasOne(d => d.ClienteGea) // GeaCorpicoDNI has ONE ClienteGea (navigation property)
            .WithMany() // ClienteGea has MANY GeaCorpicoDNI records (no specific navigation property back)
            .HasForeignKey(d => d.CLD_CLIENTE) // The foreign key in GeaCorpicoDNI
            .IsRequired(false); // Adjust based on whether CLD_CLIENTE can be NULL in your DB

        // Configuration for GeaCorpicoCliente (CLIENTE)
        modelBuilder.Entity<GeaCorpicoCliente>()
            .ToTable("CLIENTE", schema: "dbo"); // You mentioned the table is 'CLIENTE', not 'CLIENTES' here.

        base.OnModelCreating(modelBuilder);
    }
}