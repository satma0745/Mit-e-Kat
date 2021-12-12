namespace Mitekat.Discovery.Application.Persistence.EntityTypeConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mitekat.Discovery.Domain.Aggregates.Meetup;

internal class MeetupEntityTypeConfiguration : IEntityTypeConfiguration<Meetup>
{
    public void Configure(EntityTypeBuilder<Meetup> entity)
    {
        entity.ToTable("meetups");

        entity
            .HasKey(meetup => meetup.Id)
            .HasName("pk_meetups");

        entity
            .Property(meetup => meetup.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        entity
            .Property(meetup => meetup.Title)
            .HasColumnName("title");

        entity
            .Property(meetup => meetup.Description)
            .HasColumnName("description");

        entity
            .Property(meetup => meetup.Speaker)
            .HasColumnName("speaker");

        entity
            .Property(meetup => meetup.Duration)
            .HasColumnName("duration");

        entity
            .Property(meetup => meetup.StartTime)
            .HasColumnName("start_time");
    }
}
