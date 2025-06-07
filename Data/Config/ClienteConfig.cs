using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class ClienteConfig : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("clientes");
        builder.HasIndex(e => e.Dni, "clientes_dni_unique").IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Celular)
            .HasMaxLength(20)
            .HasColumnName("celular");
        builder.Property(e => e.Dni)
            .HasMaxLength(10)
            .HasColumnName("dni");
        builder.Property(e => e.Email)
            .HasMaxLength(50)
            .HasColumnName("email");
        builder.Property(e => e.Titular)
            .HasMaxLength(50)
            .HasColumnName("titular");
    }
}