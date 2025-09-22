using SharedKernel;

namespace Stores.Domain.Entities;
public class StoreAddress : BaseEntity
{
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }
}
