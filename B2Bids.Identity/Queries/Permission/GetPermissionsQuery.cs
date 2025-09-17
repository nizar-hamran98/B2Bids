using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetPermissionsQuery : IQuery<List<Permissions>>
{

    public bool AsNoTracking { get; set; }
    public string Name { get; set; }
  

}

public class GetPermissionsQueryHandler(IRepositoryBase<Permissions> permissionRepository) : IQueryHandler<GetPermissionsQuery, List<Permissions>>
{
    public Task<List<Permissions>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Permissions> db = permissionRepository.GetAll().Where(u => u.StatusId != (short)EntityStatus.Deleted);
        if (query.AsNoTracking)
        {
            db = db.AsNoTracking();
        }

        if (!string.IsNullOrEmpty(query.Name))
        {
            db = db.Where(x => x.Name == query.Name);
        }

        return permissionRepository.ToListAsync(db);
    }
}
