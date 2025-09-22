using SharedKernel;

namespace Products.Domain.Models;
public class CategoryModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public EntityStatus Status { get; set; }
}

