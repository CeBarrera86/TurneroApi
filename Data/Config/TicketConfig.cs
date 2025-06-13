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
        builder.HasIndex(e => e.EstadoId, "estado_id");
        builder.HasIndex(e => new { e.SectorIdActual, e.EstadoId }, "idx_ticket_sector_estado");
        builder.HasIndex(e => e.ClienteId, "idx_tickets_cliente");
        builder.HasIndex(e => new { e.Letra, e.Numero, e.Fecha }).IsUnique().HasDatabaseName("unique_ticket_per_day");
        builder.HasIndex(e => e.SectorIdOrigen, "sector_id_origen");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Actualizado)
            .HasColumnType("datetime")
            .HasColumnName("actualizado");
        builder.Property(e => e.ClienteId)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("cliente_id");
        builder.Property(e => e.EstadoId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("estado_id");
        builder.Property(e => e.Fecha)
            .HasColumnType("datetime")
            .HasColumnName("fecha");
        builder.Property(e => e.Letra)
            .HasMaxLength(2)
            .HasColumnName("letra");
        builder.Property(e => e.Numero)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("numero");
        builder.Property(e => e.SectorIdActual)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("sector_id_actual")
            .IsRequired(false);
        builder.Property(e => e.SectorIdOrigen)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("sector_id_origen");
        builder.HasOne(d => d.ClienteNavigation).WithMany(p => p.Ticket)
            .HasForeignKey(d => d.ClienteId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tickets_ibfk_1");
        builder.HasOne(d => d.EstadoNavigation).WithMany(p => p.Ticket)
            .HasForeignKey(d => d.EstadoId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tickets_ibfk_4");
        builder.HasOne(d => d.SectorIdActualNavigation).WithMany(p => p.TicketsSectorIdActualNavigation)
            .HasForeignKey(d => d.SectorIdActual)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tickets_ibfk_3");
        builder.HasOne(d => d.SectorIdOrigenNavigation).WithMany(p => p.TicketsSectorIdOrigenNavigation)
            .HasForeignKey(d => d.SectorIdOrigen)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tickets_ibfk_2");
    }
}