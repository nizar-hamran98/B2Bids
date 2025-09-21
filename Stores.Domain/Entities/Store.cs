using Kernel.Enums;
using SharedKernel;

namespace Stores.Domain.Entities;
public class Store : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public byte[]? Logo { get; set; }
    public string? StoreRegNo { get; set; }
    public List<byte[]>? StoreRegNoImage { get; set; }
    public bool IsAuthenticated { get; set; } = false;
    public StoreType StoreType { get; set; }
    public long StoreAddressId { get; set; }
    //public StoreAddress StoreAddress { get; set; }
}
