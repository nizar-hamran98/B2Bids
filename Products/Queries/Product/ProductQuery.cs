using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Products.Application.Queries;
public class ProductQuery : IQuery<IEnumerable<Domain.Entities.Product>>
{
    public bool AsNoTracking { get; set; }
    public long? Id { get; set; }
    public string? Name { get; set; }
    public long? StoreId { get; set; }
    public long? CategoryId { get; set; }

    public class ProductQueryHandler(IRepositoryBase<Domain.Entities.Product> repo) : IQueryHandler<ProductQuery, IEnumerable<Domain.Entities.Product>>
    {
        public async Task<IEnumerable<Domain.Entities.Product>> Handle(ProductQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Domain.Entities.Product> result = repo.GetAll().AsNoTracking();

            if(query.AsNoTracking)
                result = result.AsNoTracking();

            if (query.Id.HasValue)
                result = result.Where(login => login.Id == query.Id);

            if (query.StoreId.HasValue)
                result = result.Where(login => login.StoreId == query.StoreId);

            if (query.CategoryId.HasValue)
                result = result.Where(login => login.CategoryId == query.CategoryId);

            if (!string.IsNullOrEmpty(query.Name))
                result = result.Where(login => login.Name == query.Name);

            return await result.ToListAsync();
        }
    }
}

