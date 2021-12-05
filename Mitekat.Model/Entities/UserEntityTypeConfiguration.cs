namespace Mitekat.Model.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("users");

        entity
            .HasKey(user => user.Id)
            .HasName("pk_users");

        entity
            .HasIndex(user => user.Username)
            .IsUnique()
            .HasDatabaseName("uix_users_username");

        entity
            .Property(user => user.Id)
            .HasColumnName("id");

        entity
            .Property(user => user.Username)
            .HasColumnName("username");

        entity
            .Property(user => user.DisplayName)
            .HasColumnName("display_name");

        entity
            .Property(user => user.Password)
            .HasColumnName("password");
    }
}
