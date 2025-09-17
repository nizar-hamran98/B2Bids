using Microsoft.EntityFrameworkCore;

namespace Bid.Infrastructure;
public sealed class BidDbContext(DbContextOptions<BidDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BidDbContext).Assembly);
    }
}