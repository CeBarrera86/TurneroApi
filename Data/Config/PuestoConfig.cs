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
        builder.HasIndex(e => e.Login, "puestos_login_index");
        builder.HasIndex(e => e.Mostrador, "puestos_mostrador_foreign");
        builder.HasIndex(e => e.User, "puestos_user_foreign");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Login)
            .HasColumnType("datetime")
            .HasColumnName("login");
        builder.Property(e => e.Logout)
            .HasColumnType("datetime")
            .HasColumnName("logout");
        builder.Property(e => e.Mostrador)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("mostrador");
        builder.Property(e => e.User)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("user");
        builder.HasOne(d => d.MostradorNavigation).WithMany(p => p.Puesto)
            .HasForeignKey(d => d.Mostrador)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("puestos_mostrador_foreign");
        builder.HasOne(d => d.UserNavigation).WithMany(p => p.Puesto)
            .HasForeignKey(d => d.User)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("puestos_user_foreign");
    }
}