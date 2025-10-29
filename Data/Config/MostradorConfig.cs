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
    builder.HasIndex(e => new { e.SectorId, e.Numero }, "sector_id").IsUnique();
    builder.HasIndex(e => e.Ip).IsUnique();
    builder.Property(e => e.Id).HasColumnType("int(10) unsigned").HasColumnName("id");
    builder.Property(e => e.Ip).HasMaxLength(15).HasColumnName("ip");
    builder.Property(e => e.Numero).HasColumnType("int(10) unsigned").HasColumnName("numero");
    builder.Property(e => e.SectorId).HasColumnType("int(10) unsigned").HasColumnName("sector_id");
    builder.Property(e => e.Tipo).HasMaxLength(10).HasColumnName("tipo");
    builder.HasOne(d => d.SectorNavigation).WithMany(p => p.Mostrador).HasForeignKey(d => d.SectorId).OnDelete(DeleteBehavior.ClientSetNull)
    .HasConstraintName("mostradores_ibfk_1");
  }
}