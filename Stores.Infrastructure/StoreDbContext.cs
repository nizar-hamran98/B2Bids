using Microsoft.EntityFrameworkCore;

namespace Stores.Infrastructure;
public sealed class StoreDbContext(DbContextOptions<StoreDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
    }
}