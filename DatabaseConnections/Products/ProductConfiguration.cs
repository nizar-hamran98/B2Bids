using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseConnections;
public class ProductConfiguration : IEntityTypeConfiguration<Products.Domain.Entities.Product>
{
    public void Configure(EntityTypeBuilder<Products.Domain.Entities.Product> builder)
    {
        builder.ToTable("Product");
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
