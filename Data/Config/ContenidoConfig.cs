using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class ContenidoConfig : IEntityTypeConfiguration<Contenido>
{
  public void Configure(EntityTypeBuilder<Contenido> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("contenidos", t => t.HasCheckConstraint("ck_contenidos_tipo", "tipo IN ('imagen','video')"));
    builder.HasIndex(e => e.Nombre).IsUnique();
    builder.Property(e => e.Id).HasColumnType("int unsigned").HasColumnName("id");
    builder.Property(e => e.Nombre).HasMaxLength(100).HasColumnName("nombre");
    builder.Property(e => e.Ruta).HasMaxLength(255).HasColumnName("ruta");
    builder.Property(e => e.Tipo).HasColumnName("tipo");
    builder.Property(e => e.Activo).HasColumnType("tinyint(1)").HasColumnName("activo").HasDefaultValue(true);
    builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    builder.Property(e => e.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
  }
}
