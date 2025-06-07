using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class SectorConfig : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("sectores");
        builder.HasIndex(e => e.Letra, "sectores_letra_unique").IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Descripcion)
            .HasMaxLength(150)
            .HasColumnName("descripcion");
        builder.Property(e => e.Letra)
            .HasMaxLength(2)
            .HasColumnName("letra");
        builder.Property(e => e.Nombre)
            .HasMaxLength(25)
            .HasColumnName("nombre");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
    }
}