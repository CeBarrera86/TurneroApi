using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class TurnoConfig : IEntityTypeConfiguration<Turno>
{
  public void Configure(EntityTypeBuilder<Turno> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("turnos");
    builder.Property(e => e.Id).HasColumnType("bigint unsigned").HasColumnName("id");
    builder.Property(e => e.PuestoId).HasColumnType("int unsigned").HasColumnName("puesto_id");
    builder.Property(e => e.TicketId).HasColumnType("bigint unsigned").HasColumnName("ticket_id");
    builder.Property(e => e.FechaInicio).HasColumnType("datetime").HasColumnName("fecha_inicio");
    builder.Property(e => e.FechaFin).HasColumnType("datetime").HasColumnName("fecha_fin");
    builder.Property(e => e.EstadoId).HasColumnType("int unsigned").HasColumnName("estado_id");
    builder.HasOne(t => t.PuestoNavigation).WithMany(p => p.Turnos).HasForeignKey(t => t.PuestoId).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(t => t.TicketNavigation).WithMany(ti => ti.Turnos).HasForeignKey(t => t.TicketId).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(t => t.EstadoNavigation).WithMany(e => e.Turnos).HasForeignKey(t => t.EstadoId).OnDelete(DeleteBehavior.Restrict);
  }
}