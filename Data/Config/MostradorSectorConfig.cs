using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class MostradorSectorConfig : IEntityTypeConfiguration<MostradorSector>
{
  public void Configure(EntityTypeBuilder<MostradorSector> builder)
  {
    builder.ToTable("mostrador_sector");
    builder.HasKey(ms => new { ms.MostradorId, ms.SectorId });
    builder.Property(ms => ms.MostradorId).HasColumnName("mostrador_id");
    builder.Property(ms => ms.SectorId).HasColumnName("sector_id");
    builder.HasOne(ms => ms.Mostrador).WithMany(m => m.MostradorSectores).HasForeignKey(ms => ms.MostradorId).OnDelete(DeleteBehavior.Cascade);
    builder.HasOne(ms => ms.Sector).WithMany(s => s.MostradorSectores).HasForeignKey(ms => ms.SectorId).OnDelete(DeleteBehavior.Cascade);
  }
}