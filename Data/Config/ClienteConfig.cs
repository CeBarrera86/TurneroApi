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
        builder.HasIndex(e => e.Dni, "dni").IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.Dni)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("dni");
        builder.Property(e => e.Titular)
            .HasMaxLength(50)
            .HasColumnName("titular");
    }
}