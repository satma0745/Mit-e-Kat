namespace Mitekat.Auth.Application.Persistence.Context;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Mitekat.Auth.Domain.Aggregates.User;

internal class AuthContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AuthContext(DbContextOptions<AuthContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
