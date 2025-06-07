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
        builder.HasIndex(e => e.Id, "estados_id_index");
        builder.HasIndex(e => e.Letra, "estados_letra_unique").IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Descripcion)
            .HasMaxLength(15)
            .HasColumnName("descripcion");
        builder.Property(e => e.Letra)
            .HasMaxLength(2)
            .HasColumnName("letra");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
    }
}