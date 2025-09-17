namespace Caching;
public class CacheQueryOptions
{
    public long? Id { get; set; }
    public string? Code { get; set; }
    public string? Status { get; set; }
    public long? UserId { get; set; }
    public long? BranchId { get; set; }
    public string? BranchCode { get; set; }
    public bool AllIncludeDeleted { get; set; }
    public string? UserName { get; set; }
    public short? TaskStatus { get; set; }
}
