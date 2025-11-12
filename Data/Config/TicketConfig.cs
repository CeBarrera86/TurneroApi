using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class TicketConfig : IEntityTypeConfiguration<Ticket>
{
  public void Configure(EntityTypeBuilder<Ticket> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("tickets");
    builder.HasIndex(e => new { e.Letra, e.Numero, e.Fecha }).IsUnique();
    builder.Property(e => e.Id).HasColumnType("bigint unsigned").HasColumnName("id");
    builder.Property(e => e.Letra).HasMaxLength(2).HasColumnName("letra");
    builder.Property(e => e.Numero).HasColumnType("int unsigned").HasColumnName("numero");
    builder.Property(e => e.ClienteId).HasColumnType("bigint unsigned").HasColumnName("cliente_id");
    builder.Property(e => e.Fecha).HasColumnType("datetime").HasColumnName("fecha").HasDefaultValueSql("CURRENT_TIMESTAMP");
    builder.Property(e => e.SectorIdOrigen).HasColumnType("int unsigned").HasColumnName("sector_id_origen");
    builder.Property(e => e.SectorIdActual).HasColumnType("int unsigned").HasColumnName("sector_id_actual");
    builder.Property(e => e.EstadoId).HasColumnType("int unsigned").HasColumnName("estado_id");
    builder.Property(e => e.Actualizado).HasColumnType("datetime").HasColumnName("actualizado");
    builder.HasOne(t => t.ClienteNavigation).WithMany(c => c.Tickets).HasForeignKey(t => t.ClienteId).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(t => t.SectorIdOrigenNavigation).WithMany(s => s.TicketsSectorIdOrigenNavigation).HasForeignKey(t => t.SectorIdOrigen).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(t => t.SectorIdActualNavigation).WithMany(s => s.TicketsSectorIdActualNavigation).HasForeignKey(t => t.SectorIdActual).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(t => t.EstadoNavigation).WithMany(e => e.Tickets).HasForeignKey(t => t.EstadoId).OnDelete(DeleteBehavior.Restrict);
  }
}