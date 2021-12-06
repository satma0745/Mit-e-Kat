namespace Mitekat.Application.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Mitekat.Application.Persistence.Context;

[DbContext(typeof(MitekatContext))]
internal class MitekatContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .UseIdentityByDefaultColumns();

        modelBuilder.Entity("Mitekat.Model.Entities.Meetup", meetupEntity =>
        {
            meetupEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid")
                .HasColumnName("id");

            meetupEntity
                .Property<string>("Description")
                .HasColumnType("text")
                .HasColumnName("description");

            meetupEntity
                .Property<TimeSpan>("Duration")
                .HasColumnType("interval")
                .HasColumnName("duration");

            meetupEntity
                .Property<string>("Speaker")
                .HasColumnType("text")
                .HasColumnName("speaker");

            meetupEntity
                .Property<DateTime>("StartTime")
                .HasColumnType("timestamp with time zone")
                .HasColumnName("start_time");

            meetupEntity
                .Property<string>("Title")
                .HasColumnType("text")
                .HasColumnName("title");

            meetupEntity
                .HasKey("Id")
                .HasName("pk_meetups");

            meetupEntity.ToTable("meetups");
        });

        modelBuilder.Entity("Mitekat.Model.Entities.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .Property<Guid>("TokenId")
                .ValueGeneratedOnAdd()
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

        modelBuilder.Entity("Mitekat.Model.Entities.User", userEntity =>
        {
            userEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
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

        modelBuilder.Entity("Mitekat.Model.Entities.RefreshToken", refreshTokenEntity =>
        {
            refreshTokenEntity
                .HasOne("Mitekat.Model.Entities.User", null)
                .WithMany()
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired()
                .HasConstraintName("fk_refresh_tokens_users");
        });
    }
}
