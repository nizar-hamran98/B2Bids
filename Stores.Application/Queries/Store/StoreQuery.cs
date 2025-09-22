using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Stores.Domain.Entities;

namespace Stores.Application.Queries;
public class StoreQuery : IQuery<IEnumerable<Store>>
{
    public long? Id { get; set; }
    public bool AsNoTracking { get; set; }
    public string? Name { get; set; }
    public bool IncludeShortAddress { get; set; }

    public class StoreQueryHandler(IRepositoryBase<Store> repo) : IQueryHandler<StoreQuery, IEnumerable<Store>>
    {
        public async Task<IEnumerable<Store>> Handle(StoreQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Store> result = repo.GetAll().AsNoTracking();

            if (query.AsNoTracking)
                result = result.AsNoTracking();

            if (query.IncludeShortAddress)
                result = result.Include(x => x.StoreAddress);

            if (query.Id.HasValue)
                result = result.Where(x => x.Id == query.Id);

            if (!string.IsNullOrEmpty(query.Name))
                result = result.Where(s => s.Name == query.Name);

            return await result.ToListAsync();
        }
    }
}