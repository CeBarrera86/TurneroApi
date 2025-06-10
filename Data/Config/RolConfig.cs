using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class RolConfig : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("roles");
        builder.HasIndex(e => e.Tipo, "tipo").IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Tipo)
            .HasMaxLength(20)
            .HasColumnName("tipo");
    }
}