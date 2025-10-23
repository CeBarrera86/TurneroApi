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
        builder.HasIndex(e => e.PadreId, "padre_id");
        builder.HasIndex(e => e.Letra).IsUnique();
        builder.HasIndex(e => e.Nombre).IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Descripcion)
            .HasMaxLength(120)
            .HasColumnName("descripcion");
        builder.Property(e => e.Activo)
            .HasColumnType("tinyint(1)")
            .HasColumnName("activo")
            .IsRequired();
        builder.Property(e => e.Letra)
            .HasMaxLength(3)
            .HasColumnName("letra");
        builder.Property(e => e.Nombre)
            .HasMaxLength(50)
            .HasColumnName("nombre");
        builder.Property(e => e.PadreId)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("padre_id");
        builder.HasOne(d => d.Padre).WithMany(p => p.InversePadre)
            .HasForeignKey(d => d.PadreId)
            .HasConstraintName("sectores_ibfk_1");
    }
}