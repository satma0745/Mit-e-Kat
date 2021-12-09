namespace Mitekat.Persistence.Context;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Mitekat.Domain.Aggregates.Meetup;
using Mitekat.Domain.Aggregates.User;

internal class MitekatContext : DbContext
{
    public DbSet<Meetup> Meetups { get; set; }
    
    public DbSet<User> Users { get; set; }

    public MitekatContext(DbContextOptions<MitekatContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
