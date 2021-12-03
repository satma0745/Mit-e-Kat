namespace Mitekat.Model.Migrations;

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Mitekat.Model.Context;

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
    }
}
