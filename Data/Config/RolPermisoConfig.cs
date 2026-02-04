using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class RolPermisoConfig : IEntityTypeConfiguration<RolPermiso>
{
  public void Configure(EntityTypeBuilder<RolPermiso> builder)
  {
    builder.ToTable("rol_permiso");
    builder.HasKey(rp => new { rp.RolId, rp.PermisoId });
    builder.Property(rp => rp.RolId).HasColumnType("int unsigned").HasColumnName("rol_id");
    builder.Property(rp => rp.PermisoId).HasColumnType("int unsigned").HasColumnName("permiso_id");
    builder.HasOne(rp => rp.Rol).WithMany(r => r.RolPermisos).HasForeignKey(rp => rp.RolId).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(rp => rp.Permiso).WithMany(p => p.RolPermisos).HasForeignKey(rp => rp.PermisoId).OnDelete(DeleteBehavior.Restrict);
  }
}