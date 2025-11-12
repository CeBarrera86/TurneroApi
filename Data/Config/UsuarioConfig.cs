using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
{
  public void Configure(EntityTypeBuilder<Usuario> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("usuarios");
    builder.HasIndex(e => e.Username).IsUnique();
    builder.Property(e => e.Id).HasColumnType("int unsigned").HasColumnName("id");
    builder.Property(e => e.Nombre).HasMaxLength(50).HasColumnName("nombre");
    builder.Property(e => e.Apellido).HasMaxLength(50).HasColumnName("apellido");
    builder.Property(e => e.Username).HasMaxLength(30).HasColumnName("username");
    builder.Property(e => e.RolId).HasColumnType("int unsigned").HasColumnName("rol_id");
    builder.Property(e => e.Activo).HasColumnType("tinyint(1)").HasColumnName("activo").HasDefaultValue(true);
    builder.Property(e => e.CreatedAt).HasColumnType("datetime").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    builder.Property(e => e.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
    builder.HasOne(u => u.RolNavigation).WithMany(r => r.Usuarios).HasForeignKey(u => u.RolId).OnDelete(DeleteBehavior.Cascade);
  }
}
