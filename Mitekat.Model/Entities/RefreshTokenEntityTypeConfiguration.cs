namespace Mitekat.Model.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.ToTable("refresh_tokens");

        entity
            .HasKey(refreshToken => refreshToken.TokenId)
            .HasName("pk_refresh_tokens");

        entity
            .HasIndex(refreshToken => refreshToken.UserId)
            .HasDatabaseName("ix_refresh_tokens_user_id");

        entity
            .Property(refreshToken => refreshToken.TokenId)
            .HasColumnName("token_id");

        entity
            .Property(refreshToken => refreshToken.UserId)
            .HasColumnName("user_id");

        entity
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .HasConstraintName("fk_refresh_tokens_users");
    }
}
