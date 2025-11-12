using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class MostradorConfig : IEntityTypeConfiguration<Mostrador>
{
  public void Configure(EntityTypeBuilder<Mostrador> builder)
  {
    builder.HasKey(m => m.Id).HasName("PRIMARY");
    builder.ToTable("mostradores");
    builder.HasIndex(m => m.Ip).IsUnique();
    builder.HasIndex(m => new { m.Numero, m.Ip }).IsUnique();
    builder.Property(m => m.Id).HasColumnType("int unsigned").HasColumnName("id");
    builder.Property(m => m.Numero).HasColumnType("int unsigned").HasColumnName("numero");
    builder.Property(m => m.Ip).HasMaxLength(15).HasColumnName("ip");
    builder.Property(m => m.Tipo).HasMaxLength(10).HasColumnName("tipo");
    builder.Property(m => m.CreatedAt).HasColumnType("datetime").HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
    builder.Property(m => m.UpdatedAt).HasColumnType("datetime").HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
  }
}