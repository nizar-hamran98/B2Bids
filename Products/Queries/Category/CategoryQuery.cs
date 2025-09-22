using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using SharedKernel;

namespace Products.Application.Queries;
public class CategoryQuery : IQuery<IEnumerable<Category>>
{
    public long? Id { get; set; }
    public bool AsNoTracking { get; set; }
    public string? Name { get; set; }

    public class CategoryQueryHandler(IRepositoryBase<Category> repo) : IQueryHandler<CategoryQuery, IEnumerable<Category>>
    {
        public async Task<IEnumerable<Category>> Handle(CategoryQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> result = repo.GetAll().AsNoTracking();

            if(query.AsNoTracking)
                result = result.AsNoTracking();

            if (query.Id.HasValue)
                result = result.Where(x => x.Id == query.Id);

            if (!string.IsNullOrEmpty(query.Name))
                result = result.Where(login => login.Name == query.Name);
          
            return await result.ToListAsync();
        }
    }

}
