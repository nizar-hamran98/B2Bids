using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Stores.Domain.Entities;

namespace Stores.Application.Queries;
public class StoreAddressQuery : IQuery<IEnumerable<StoreAddress>>
{
    public long? Id { get; set; }
    public bool AsNoTracking { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }

    public class StoreAddressQueryHandler(IRepositoryBase<StoreAddress> repo) : IQueryHandler<StoreAddressQuery, IEnumerable<StoreAddress>>
    {
        public async Task<IEnumerable<StoreAddress>> Handle(StoreAddressQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<StoreAddress> result = repo.GetAll().AsNoTracking();

            if (query.AsNoTracking)
                result = result.AsNoTracking();

            if (query.Id.HasValue)
                result = result.Where(x => x.Id == query.Id);

            if (!string.IsNullOrEmpty(query.Country))
                result = result.Where(s => s.Country == query.Country);

            if (!string.IsNullOrEmpty(query.City))
                result = result.Where(s => s.City == query.City);

            return await result.ToListAsync();
        }
    }
}
