using SharedKernel;

namespace Bid.Domain.Entities;
public class ProductBids : BaseEntity
{
    public decimal InitialBid { get; set; }
    public decimal LastBid { get; set; }
    public long WinnerId { get; set; } // user id
    public int TotalOfBids { get; set; }
    public DateTimeOffset EndBidDate { get; set; }
    public List<long> ListOfBidders { get; set; } // user ids
}
