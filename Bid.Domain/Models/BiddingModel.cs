using SharedKernel;

namespace Bid.Domain.Models;
public class BiddingModel
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public decimal BidPrice { get; set; }
    public long BidderId { get; set; } // user id
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public EntityStatus Status { get; set; }
}
