using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class HistorialConfig : IEntityTypeConfiguration<Historial>
{
  public void Configure(EntityTypeBuilder<Historial> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("historiales");
    builder.Property(e => e.Id).HasColumnType("bigint unsigned").HasColumnName("id");
    builder.Property(e => e.TicketId).HasColumnType("bigint unsigned").HasColumnName("ticket_id");
    builder.Property(e => e.EstadoId).HasColumnType("int unsigned").HasColumnName("estado_id");
    builder.Property(e => e.Fecha).HasColumnType("datetime").HasColumnName("fecha");
    builder.Property(e => e.PuestoId).HasColumnType("int unsigned").HasColumnName("puesto_id");
    builder.Property(e => e.TurnoId).HasColumnType("bigint unsigned").HasColumnName("turno_id");
    builder.Property(e => e.UsuarioId).HasColumnType("int unsigned").HasColumnName("usuario_id");
    builder.Property(e => e.Comentarios).HasMaxLength(255).HasColumnName("comentarios");
    builder.HasOne(h => h.TicketNavigation).WithMany(t => t.Historiales).HasForeignKey(h => h.TicketId).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(h => h.EstadoNavigation).WithMany(e => e.Historiales).HasForeignKey(h => h.EstadoId).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(h => h.PuestoNavigation).WithMany(p => p.Historiales).HasForeignKey(h => h.PuestoId).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(h => h.TurnoNavigation).WithMany(t => t.Historiales).HasForeignKey(h => h.TurnoId).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(h => h.UsuarioNavigation).WithMany(u => u.Historiales).HasForeignKey(h => h.UsuarioId).OnDelete(DeleteBehavior.Restrict);
    builder.HasIndex(e => e.TicketId).HasDatabaseName("idx_historial_ticket");
    builder.HasIndex(e => e.Fecha).HasDatabaseName("idx_historial_fecha");
  }
}
