using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class TareaConfig : IEntityTypeConfiguration<Tarea>
{
    public void Configure(EntityTypeBuilder<Tarea> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("tareas");
        builder.HasIndex(e => e.Sector, "tareas_sector_foreign");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Descripcion)
            .HasMaxLength(45)
            .HasColumnName("descripcion");
        builder.Property(e => e.Sector)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("sector");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
        builder.HasOne(d => d.SectorNavigation).WithMany(p => p.Tarea)
            .HasForeignKey(d => d.Sector)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("tareas_sector_foreign");
    }
}