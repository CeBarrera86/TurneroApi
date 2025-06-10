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
        builder.HasIndex(e => e.UsuarioId, "idx_puestos_usuario");
        builder.HasIndex(e => e.MostradorId, "mostrador_id");
        builder.Property(e => e.Id)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Activo)
            .IsRequired()
            .HasDefaultValueSql("'1'")
            .HasColumnName("activo");
        builder.Property(e => e.Login)
            .HasColumnType("datetime")
            .HasColumnName("login");
        builder.Property(e => e.Logout)
            .HasColumnType("datetime")
            .HasColumnName("logout");
        builder.Property(e => e.MostradorId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("mostrador_id");
        builder.Property(e => e.UsuarioId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("usuario_id");
        builder.HasOne(d => d.MostradorNavigation).WithMany(p => p.Puesto)
            .HasForeignKey(d => d.MostradorId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("puestos_ibfk_2");
        builder.HasOne(d => d.UsuarioNavigation).WithMany(p => p.Puesto)
            .HasForeignKey(d => d.UsuarioId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("puestos_ibfk_1");
    }
}