using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class PuestoConfig : IEntityTypeConfiguration<Puesto>
{
  public void Configure(EntityTypeBuilder<Puesto> builder)
  {
    builder.HasKey(e => e.Id).HasName("PRIMARY");
    builder.ToTable("puestos");
    builder.Property(e => e.Id).HasColumnType("int unsigned").HasColumnName("id");
    builder.Property(e => e.UsuarioId).HasColumnType("int unsigned").HasColumnName("usuario_id");
    builder.Property(e => e.MostradorId).HasColumnType("int unsigned").HasColumnName("mostrador_id");
    builder.Property(e => e.Login).HasColumnType("datetime").HasColumnName("login");
    builder.Property(e => e.Logout).HasColumnType("datetime").HasColumnName("logout");
    builder.Property(e => e.Activo).HasColumnType("tinyint(1)").HasColumnName("activo").HasDefaultValue(true);
    builder.HasOne(p => p.UsuarioNavigation).WithMany(u => u.Puestos).HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.Restrict);
    builder.HasOne(p => p.MostradorNavigation).WithMany(m => m.Puestos).HasForeignKey(p => p.MostradorId).OnDelete(DeleteBehavior.Restrict);
    builder.HasIndex(e => e.UsuarioId).HasDatabaseName("idx_puestos_usuario");
  }
}