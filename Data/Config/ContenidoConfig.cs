using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class ContenidoConfig : IEntityTypeConfiguration<Contenido>
{
  public void Configure(EntityTypeBuilder<Contenido> builder)
  {
    builder.HasKey(p => p.Id).HasName("PRIMARY");
    builder.ToTable("contenidos");
    builder.Property(p => p.Id).HasColumnType("int unsigned").HasColumnName("id");
    builder.Property(p => p.Nombre).HasMaxLength(100).HasColumnName("nombre");
    builder.Property(p => p.Ruta).HasMaxLength(255).HasColumnName("ruta");
    builder.Property(p => p.Tipo).HasMaxLength(10).HasColumnName("tipo");
    builder.Property(p => p.Activa).HasColumnName("activa");
    builder.Property(p => p.Fecha).HasColumnName("fecha").HasDefaultValueSql("CURRENT_TIMESTAMP");
    builder.HasIndex(p => p.Nombre).IsUnique();
  }
}
