namespace Mitekat.Discovery.Application.Persistence.Context;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Mitekat.Discovery.Domain.Aggregates.Meetup;

internal class DiscoveryContext : DbContext
{
    public DbSet<Meetup> Meetups { get; set; }

    public DiscoveryContext(DbContextOptions<DiscoveryContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
