namespace Mitekat.Auth.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mitekat.Auth.Persistence.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

[DbContext(typeof(AuthContext))]
[Migration("20211209173137_AddUsersAndRefreshTokens")]
partial class AddUsersAndRefreshTokens
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .UseIdentityByDefaultColumns();

        modelBuilder.Entity("Mitekat.Auth.Domain.Aggregates.User.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .Property<Guid>("TokenId")
                .HasColumnType("uuid")
                .HasColumnName("token_id");

            refreshTokenEntity
                .Property<Guid>("UserId")
                .HasColumnType("uuid")
                .HasColumnName("user_id");

            refreshTokenEntity
                .HasKey("TokenId")
                .HasName("pk_refresh_tokens");

            refreshTokenEntity
                .HasIndex("UserId")
                .HasDatabaseName("ix_refresh_tokens_user_id");

            refreshTokenEntity.ToTable("refresh_tokens");
        });

        modelBuilder.Entity("Mitekat.Auth.Domain.Aggregates.User.User", userEntity =>
        {
            userEntity
                .Property<Guid>("Id")
                .HasColumnType("uuid")
                .HasColumnName("id");

            userEntity
                .Property<string>("DisplayName")
                .HasColumnType("text")
                .HasColumnName("display_name");

            userEntity
                .Property<string>("Password")
                .HasColumnType("text")
                .HasColumnName("password");

            userEntity
                .Property<string>("Username")
                .HasColumnType("text")
                .HasColumnName("username");

            userEntity
                .HasKey("Id")
                .HasName("pk_users");

            userEntity
                .HasIndex("Username")
                .IsUnique()
                .HasDatabaseName("uix_users_username");

            userEntity.ToTable("users");
        });

        modelBuilder.Entity("Mitekat.Auth.Domain.Aggregates.User.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .HasOne("Mitekat.Auth.Domain.Aggregates.User.User", null)
                .WithMany("RefreshTokens")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_refresh_tokens_users");
        });

        modelBuilder.Entity("Mitekat.Auth.Domain.Aggregates.User.User", userEntity =>
        {
            userEntity.Navigation("RefreshTokens");
        });
    }
}
