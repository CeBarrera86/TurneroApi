using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class HistorialConfig : IEntityTypeConfiguration<Historial>
{
    public void Configure(EntityTypeBuilder<Historial> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("historiales");
        builder.HasIndex(e => e.DerPara, "historiales_der_para_foreign");
        builder.HasIndex(e => e.Estado, "historiales_estado_foreign");
        builder.HasIndex(e => e.Puesto, "historiales_puesto_foreign");
        builder.HasIndex(e => e.Turno, "historiales_turno_foreign");
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.DerPara)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("der_para");
        builder.Property(e => e.Estado)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("estado");
        builder.Property(e => e.Puesto)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("puesto");
        builder.Property(e => e.Turno)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("turno");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
        builder.HasOne(d => d.DerParaNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.DerPara)
            .HasConstraintName("historiales_der_para_foreign");
        builder.HasOne(d => d.EstadoNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.Estado)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("historiales_estado_foreign");
        builder.HasOne(d => d.PuestoNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.Puesto)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("historiales_puesto_foreign");
        builder.HasOne(d => d.TurnoNavigation).WithMany(p => p.Historial)
            .HasForeignKey(d => d.Turno)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("historiales_turno_foreign");
    }
}