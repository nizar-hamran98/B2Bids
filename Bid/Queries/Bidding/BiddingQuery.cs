using Bid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Bid.Application.Queries;
public class BiddingQuery : IQuery<IEnumerable<Bidding>>
{
    public long? Id { get; set; }
    public bool AsNoTracking { get; set; }
    public long? ProductId { get; set; }
    public long? BidderId { get; set; }

    public class BiddingQueryHandler(IRepositoryBase<Bidding> repo) : IQueryHandler<BiddingQuery, IEnumerable<Bidding>>
    {
        public async Task<IEnumerable<Bidding>> Handle(BiddingQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Bidding> result = repo.GetAll().AsNoTracking();

            if (query.AsNoTracking)
                result = result.AsNoTracking();

            if (query.Id.HasValue)
                result = result.Where(x => x.Id == query.Id);

             if (query.BidderId.HasValue)
                result = result.Where(x => x.BidderId == query.BidderId);
              
            if (query.ProductId.HasValue)
                result = result.Where(x => x.ProductId == query.ProductId);

            return await result.ToListAsync();
        }
    }
}
