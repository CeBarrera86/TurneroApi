using Microsoft.EntityFrameworkCore;
using TurneroApi.Models.GeaPico;

namespace TurneroApi.Data;

public class GeaCorpicoDbContext : DbContext
{
    public GeaCorpicoDbContext(DbContextOptions<GeaCorpicoDbContext> options) : base(options) { }
    public DbSet<GeaCorpicoDNI> GeaCorpicoClienteDocumentos { get; set; } = null!;
    public DbSet<GeaCorpicoCliente> GeaCorpicoClientes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GeaCorpicoDNI>(entity =>
        {
            entity.ToTable("CLIENTE_DOCUMENTO", schema: "dbo");
            // Clave primaria compuesta para CLIENTE_DOCUMENTO
            entity.HasKey(cd => new { cd.CLD_EMPRESA, cd.CLD_CLIENTE, cd.CLD_TIPO_DOCUMENTO });
            // Relación Many-to-One: GeaCorpicoDNI (muchos) a GeaCorpicoCliente (uno)
            // La clave foránea en GeaCorpicoDNI es CLD_CLIENTE.
            // La clave principal a la que se refiere CLD_CLIENTE en GeaCorpicoCliente es CLI_ID.
            entity.HasOne(d => d.ClienteGea)
                  .WithMany()
                  .HasForeignKey(d => new { d.CLD_EMPRESA, d.CLD_CLIENTE })
                  .HasPrincipalKey(c => new { c.CLI_EMPRESA, c.CLI_ID })
                  .IsRequired();
        });


        // Entidad GeaCorpicoCliente (tabla CLIENTE)
        modelBuilder.Entity<GeaCorpicoCliente>(entity =>
        {
            entity.ToTable("CLIENTE", schema: "dbo");
            // Clave primaria compuesta para CLIENTE
            entity.HasKey(c => new { c.CLI_EMPRESA, c.CLI_ID });
        });

        base.OnModelCreating(modelBuilder);
    }
}