namespace Mitekat.Auth.Application.Persistence.EntityTypeConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mitekat.Auth.Domain.Aggregates.User;

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
            .HasColumnName("token_id")
            .ValueGeneratedNever();

        entity
            .Property(refreshToken => refreshToken.UserId)
            .HasColumnName("user_id");

        entity
            .HasOne<User>()
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .HasConstraintName("fk_refresh_tokens_users");
    }
}
