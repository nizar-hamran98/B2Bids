using Bid.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Bid.Application.Queries;
public class ProductBidsQuery : IQuery<IEnumerable<ProductBids>>
{
    public long? Id { get; set; }
    public bool AsNoTracking { get; set; }
    public long? WinnerId { get; set; }
    public long? ListOfBidders { get; set; }

    public class ProductBidsQueryHandler(IRepositoryBase<ProductBids> repo) : IQueryHandler<ProductBidsQuery, IEnumerable<ProductBids>>
    {
        public async Task<IEnumerable<ProductBids>> Handle(ProductBidsQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<ProductBids> result = repo.GetAll().AsNoTracking();

            if (query.AsNoTracking)
                result = result.AsNoTracking();

            if (query.Id.HasValue)
                result = result.Where(x => x.Id == query.Id);

            if (query.WinnerId.HasValue)
                result = result.Where(x => x.WinnerId == query.WinnerId);

             if (query.ListOfBidders.HasValue)
                result = result.Where(x => x.ListOfBidders.Contains((long)query.ListOfBidders));

            return await result.ToListAsync();
        }
    }
}

