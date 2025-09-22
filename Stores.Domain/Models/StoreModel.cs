using Kernel.Enums;
using SharedKernel;
using Stores.Domain.Entities;

namespace Stores.Domain.Models;
public class StoreModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public byte[]? Logo { get; set; } // needs to be changes
    public string? StoreRegNo { get; set; }
    public List<string>? StoreRegNoImage { get; set; } // needs to be changes
    public bool IsAuthenticated { get; set; } = false;
    public StoreType StoreType { get; set; }
    public long StoreAddressId { get; set; }
    public StoreAddress? StoreAddress { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public EntityStatus Status { get; set; }
}
