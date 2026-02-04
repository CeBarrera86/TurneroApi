using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class EstadoConfig : IEntityTypeConfiguration<Estado>
{
  public void Configure(EntityTypeBuilder<Estado> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("estados", t => t.HasCheckConstraint("ck_estados_letra", "letra REGEXP '^[A-Z]{1,2}$'"));
    builder.HasIndex(e => e.Letra).IsUnique();
    builder.Property(e => e.Id).HasColumnType("int unsigned").HasColumnName("id");
    builder.Property(e => e.Letra).HasMaxLength(2).HasColumnName("letra");
    builder.Property(e => e.Descripcion).HasMaxLength(20).HasColumnName("descripcion");
    builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    builder.Property(e => e.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
  }
}
