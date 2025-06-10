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
        builder.HasIndex(e => e.RolId, "rol_id");
        builder.HasIndex(e => e.Username, "username").IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Apellido)
            .HasMaxLength(50)
            .HasColumnName("apellido");
        builder.Property(e => e.Nombre)
            .HasMaxLength(50)
            .HasColumnName("nombre");
        builder.Property(e => e.RolId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("rol_id");
        builder.Property(e => e.Username)
            .HasMaxLength(30)
            .HasColumnName("username");
        builder.HasOne(d => d.RolNavigation).WithMany(p => p.Usuario)
            .HasForeignKey(d => d.RolId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("usuarios_ibfk_1");
    }
}