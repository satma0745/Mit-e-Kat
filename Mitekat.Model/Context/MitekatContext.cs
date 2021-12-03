namespace Mitekat.Model.Context;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Mitekat.Model.Entities;

public class MitekatContext : DbContext
{
    public DbSet<Meetup> Meetups { get; set; }

    public MitekatContext(DbContextOptions<MitekatContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
