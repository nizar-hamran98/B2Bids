using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stores.Domain.Entities;

namespace DatabaseConnections;
public class StoreAddressConfiguration : IEntityTypeConfiguration<StoreAddress>
{
    public void Configure(EntityTypeBuilder<StoreAddress> builder)
    {
        builder.ToTable("StoreAddress");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}