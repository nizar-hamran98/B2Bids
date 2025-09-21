using SharedKernel;

namespace Products.Domain.Models;
public class ProductModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset ExpiresIn { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; } // url
    public long CategoryId { get; set; }
    //public Category Category { get; set; }
    public long StoreId { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public EntityStatus Status { get; set; }
    //public string StoreName { get; set; }
    //public string StoreLogo { get; set; }
}
