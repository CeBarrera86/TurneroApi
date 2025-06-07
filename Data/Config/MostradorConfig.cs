using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class MostradorConfig : IEntityTypeConfiguration<Mostrador>
{
    public void Configure(EntityTypeBuilder<Mostrador> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("mostradores");
        builder.HasIndex(e => e.Ip, "mostradores_ip_unique").IsUnique();
        builder.HasIndex(e => e.Sector, "mostradores_sector_foreign");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Alfa)
            .HasMaxLength(4)
            .HasColumnName("alfa");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Ip)
            .HasMaxLength(15)
            .HasColumnName("ip");
        builder.Property(e => e.Numero)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("numero");
        builder.Property(e => e.Sector)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("sector");
        builder.Property(e => e.Tipo)
            .HasMaxLength(10)
            .HasColumnName("tipo");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
        builder.HasOne(d => d.SectorNavigation).WithMany(p => p.Mostrador)
            .HasForeignKey(d => d.Sector)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("mostradores_sector_foreign");
    }
}