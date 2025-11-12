using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class PermisoConfig : IEntityTypeConfiguration<Permiso>
{
  public void Configure(EntityTypeBuilder<Permiso> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("permisos");
    builder.HasIndex(e => e.Nombre).IsUnique();
    builder.Property(e => e.Id).HasColumnType("int unsigned").HasColumnName("id");
    builder.Property(e => e.Nombre).HasMaxLength(50).HasColumnName("nombre");
    builder.Property(e => e.Descripcion).HasMaxLength(100).HasColumnName("descripcion");
    builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    builder.Property(e => e.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
  }
}
