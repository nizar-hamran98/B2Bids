using Kernel.Enums;
using SharedKernel;

namespace Products.Domain.Entities;
public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset ExpiresIn { get; set; }
    public string Description { get; set; }
    public List<byte[]> Images { get; set; }
    public ProductCategory Category { get; set; }
}
