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
        builder.HasIndex(e => e.Cliente, "tickets_cliente_foreign");
        builder.HasIndex(e => e.Estado, "tickets_estado_foreign");
        builder.HasIndex(e => new { e.Letra, e.Numero, e.FechaTicket }, "tickets_letra_numero_fecha_unique").IsUnique();
        builder.HasIndex(e => e.Sector, "tickets_sector_foreign");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Cliente)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("cliente");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Estado)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("estado");
        builder.Property(e => e.FechaTicket)
            .HasDefaultValueSql("curdate()")
            .HasColumnName("fecha_ticket");
        builder.Property(e => e.Letra)
            .HasMaxLength(2)
            .HasColumnName("letra");
        builder.Property(e => e.Numero)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("numero");
        builder.Property(e => e.Sector)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("sector");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
        builder.HasOne(d => d.ClienteNavigation).WithMany(p => p.Ticket)
            .HasForeignKey(d => d.Cliente)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tickets_cliente_foreign");
        builder.HasOne(d => d.EstadoNavigation).WithMany(p => p.Ticket)
            .HasForeignKey(d => d.Estado)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tickets_estado_foreign");
        builder.HasOne(d => d.SectorNavigation).WithMany(p => p.Ticket)
            .HasForeignKey(d => d.Sector)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tickets_sector_foreign");
    }
}