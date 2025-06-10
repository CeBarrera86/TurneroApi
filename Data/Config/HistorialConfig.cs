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
        builder.HasIndex(e => e.EstadoId, "estado_id");
        builder.HasIndex(e => e.Fecha, "idx_historial_fecha");
        builder.HasIndex(e => e.TicketId, "idx_historial_ticket");
        builder.HasIndex(e => e.PuestoId, "puesto_id");
        builder.HasIndex(e => e.TurnoId, "turno_id");
        builder.HasIndex(e => e.UsuarioId, "usuario_id");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Comentarios)
            .HasMaxLength(255)
            .HasColumnName("comentarios");
        builder.Property(e => e.EstadoId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("estado_id");
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .HasColumnName("fecha");
        builder.Property(e => e.PuestoId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("puesto_id");
        builder.Property(e => e.TicketId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("ticket_id");
        builder.Property(e => e.TurnoId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("turno_id");
        builder.Property(e => e.UsuarioId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("usuario_id");
        builder.HasOne(d => d.EstadoNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.EstadoId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("historiales_ibfk_2");
        builder.HasOne(d => d.PuestoNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.PuestoId)
            .HasConstraintName("historiales_ibfk_3");
        builder.HasOne(d => d.TicketNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.TicketId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("historiales_ibfk_1");
        builder.HasOne(d => d.TurnoNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.TurnoId)
            .HasConstraintName("historiales_ibfk_4");
        builder.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.UsuarioId)
            .HasConstraintName("historiales_ibfk_5");
    }
}