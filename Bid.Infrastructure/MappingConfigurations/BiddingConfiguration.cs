using Bid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bid.Infrastructure.MappingConfigurations;
public class BiddingConfiguration : IEntityTypeConfiguration<Bidding>
{
    public void Configure(EntityTypeBuilder<Bidding> builder)
    {
        builder.ToTable("Bidding");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}