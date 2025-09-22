using SharedKernel;

namespace Stores.Domain.Models;
public class StoreAddressModel
{
    public long Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string? Longitude { get; set; }
    public string? Latitude { get; set; }

    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public EntityStatus Status { get; set; }
}
