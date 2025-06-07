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
        builder.HasIndex(e => e.Puesto, "turnos_puesto_foreign");
        builder.HasIndex(e => e.Ticket, "turnos_ticket_foreign");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Puesto)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("puesto");
        builder.Property(e => e.Ticket)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("ticket");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
        builder.HasOne(d => d.PuestoNavigation).WithMany(p => p.Turno)
            .HasForeignKey(d => d.Puesto)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("turnos_puesto_foreign");
        builder.HasOne(d => d.TicketNavigation).WithMany(p => p.Turno)
            .HasForeignKey(d => d.Ticket)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("turnos_ticket_foreign");
    }
}