using Bid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseConnections;
public class ProductBidsConfiguration : IEntityTypeConfiguration<ProductBids>
{
    public void Configure(EntityTypeBuilder<ProductBids> builder)
    {
        builder.ToTable("ProductBids");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
