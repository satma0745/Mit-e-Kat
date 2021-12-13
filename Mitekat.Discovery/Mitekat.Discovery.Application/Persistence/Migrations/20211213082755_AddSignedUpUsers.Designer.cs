namespace Mitekat.Discovery.Application.Persistence.Migrations;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mitekat.Discovery.Application.Persistence.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

[DbContext(typeof(DiscoveryContext))]
[Migration("20211213082755_AddSignedUpUsers")]
partial class AddSignedUpUsers
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63)
            .UseIdentityByDefaultColumns();

        modelBuilder.Entity("Mitekat.Discovery.Domain.Aggregates.Meetup.Meetup", meetupEntity =>
        {
            meetupEntity
                .Property<Guid>("Id")
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

        modelBuilder.Entity("Mitekat.Discovery.Domain.Aggregates.Meetup.SignedUpUser", signedUpUserEntity =>
        {
            signedUpUserEntity
                .Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid")
                .HasColumnName("id");

            signedUpUserEntity
                .Property<Guid?>("MeetupId")
                .HasColumnType("uuid")
                .HasColumnName("meetup_id");

            signedUpUserEntity
                .HasKey("Id")
                .HasName("pk_signed_up_users");

            signedUpUserEntity
                .HasIndex("MeetupId")
                .IsUnique()
                .HasDatabaseName("uix_signed_up_users_meetup_id");

            signedUpUserEntity.ToTable("signed_up_users");
        });

        modelBuilder.Entity(
            "Mitekat.Discovery.Domain.Aggregates.Meetup.SignedUpUser",
            signedUpUserEntity =>
                signedUpUserEntity
                    .HasOne("Mitekat.Discovery.Domain.Aggregates.Meetup.Meetup", null)
                    .WithMany("SignedUpUsers")
                    .HasForeignKey("MeetupId")
                    .HasConstraintName("fk_meetups_signed_up_users"));

        modelBuilder.Entity(
            "Mitekat.Discovery.Domain.Aggregates.Meetup.Meetup",
            meetupEntity => meetupEntity.Navigation("SignedUpUsers"));
    }
}
