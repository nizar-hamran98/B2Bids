using SharedKernel;

namespace Bid.Domain.Entities;
public class Bidding : BaseEntity
{
    public long ProductId { get; set; }
    public decimal BidPrice { get; set; }
    public long BidderId  { get; set; } // user id
}
