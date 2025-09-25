using Microsoft.EntityFrameworkCore;

namespace DatabaseConnections;
public sealed class B2BidsDbContext(DbContextOptions<B2BidsDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(B2BidsDbContext).Assembly);
    }
}
