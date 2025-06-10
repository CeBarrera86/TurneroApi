using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class EstadoConfig : IEntityTypeConfiguration<Estado>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("estados");
        builder.Property(e => e.Id)
            .HasColumnType("int(10) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Descripcion)
            .HasMaxLength(20)
            .HasColumnName("descripcion");
        builder.Property(e => e.Letra)
            .HasMaxLength(2)
            .HasColumnName("letra");
        builder.HasIndex(e => e.Letra).IsUnique();
    }
}