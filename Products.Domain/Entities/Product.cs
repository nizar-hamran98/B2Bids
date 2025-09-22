using SharedKernel;

namespace Products.Domain.Entities;
public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset ExpiresIn { get; set; }
    public string Description { get; set; }
    public List<string> Images { get; set; } // url
    public long CategoryId { get; set; }
    //public Category Category { get; set; }
    public long StoreId { get; set; }
    //public string StoreName { get; set; }
    //public string StoreLogo { get; set; }

}
