namespace Mitekat.Discovery.Application.Persistence.EntityTypeConfigurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mitekat.Discovery.Domain.Aggregates.Meetup;

internal class SignedUpUserEntityTypeConfiguration : IEntityTypeConfiguration<SignedUpUser>
{
    public void Configure(EntityTypeBuilder<SignedUpUser> entity)
    {
        entity.ToTable("signed_up_users");

        entity
            .HasKey(user => user.Id)
            .HasName("pk_signed_up_users");

        entity
            .HasIndex("MeetupId")
            .IsUnique()
            .HasDatabaseName("uix_signed_up_users_meetup_id");

        entity
            .Property(user => user.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        entity
            .Property("MeetupId")
            .HasColumnName("meetup_id");

        entity
            .HasOne<Meetup>()
            .WithMany(meetup => meetup.SignedUpUsers)
            .HasForeignKey("MeetupId")
            .HasConstraintName("fk_meetups_signed_up_users");
    }
}
