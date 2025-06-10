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
        builder.HasIndex(e => e.EstadoId, "estado_id");
        builder.HasIndex(e => e.PuestoId, "idx_turno_puesto");
        builder.HasIndex(e => e.TicketId, "idx_turnos_ticket");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.EstadoId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("estado_id");
        builder.Property(e => e.FechaFin)
            .HasColumnType("datetime")
            .HasColumnName("fecha_fin");
        builder.Property(e => e.FechaInicio)
            .HasColumnType("datetime")
            .HasColumnName("fecha_inicio");
        builder.Property(e => e.PuestoId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("puesto_id");
        builder.Property(e => e.TicketId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("ticket_id");
        builder.HasOne(d => d.EstadoNavigation).WithMany(p => p.Turno)
            .HasForeignKey(d => d.EstadoId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("turnos_ibfk_3");
        builder.HasOne(d => d.PuestoNavigation).WithMany(p => p.Turno)
            .HasForeignKey(d => d.PuestoId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("turnos_ibfk_1");
        builder.HasOne(d => d.TicketNavigation).WithMany(p => p.Turno)
            .HasForeignKey(d => d.TicketId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("turnos_ibfk_2");
    }
}