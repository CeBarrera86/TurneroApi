using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TurneroApi.Models;

namespace TurneroApi.Data.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).HasName("PRIMARY");
        builder.ToTable("users");
        builder.HasIndex(e => e.Role, "users_role_foreign");
        builder.HasIndex(e => e.Username, "users_username_unique").IsUnique();
        builder.Property(e => e.Id)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("created_at");
        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .HasColumnName("email");
        builder.Property(e => e.EmailVerifiedAt)
            .HasColumnType("timestamp")
            .HasColumnName("email_verified_at");
        builder.Property(e => e.Name)
            .HasMaxLength(255)
            .HasColumnName("name");
        builder.Property(e => e.Password)
            .HasMaxLength(255)
            .HasColumnName("password");
        builder.Property(e => e.RememberToken)
            .HasMaxLength(100)
            .HasColumnName("remember_token");
        builder.Property(e => e.Role)
            .HasColumnType("bigint(20) unsigned")
            .HasColumnName("role");
        builder.Property(e => e.Surname)
            .HasMaxLength(50)
            .HasColumnName("surname");
        builder.Property(e => e.UpdatedAt)
            .HasColumnType("timestamp")
            .HasColumnName("updated_at");
        builder.Property(e => e.Username)
            .HasMaxLength(30)
            .HasColumnName("username");
        builder.HasOne(d => d.RoleNavigation).WithMany(p => p.User)
            .HasForeignKey(d => d.Role)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("users_role_foreign");
    }
}