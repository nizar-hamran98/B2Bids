using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetPermissionQuery : IQuery<Permissions>
{
    public long? Id { get; set; }
    public bool AsNoTracking { get; set; }
}

public class GetPermissionQueryHandler(IRepositoryBase<Permissions> permissionRepository) : IQueryHandler<GetPermissionQuery, Permissions>
{
    public Task<Permissions> Handle(GetPermissionQuery query, CancellationToken cancellationToken = default)
    {

        IQueryable<Permissions> db = permissionRepository.GetAll().Where(u => u.StatusId != (short)EntityStatus.Deleted);
        if (query.AsNoTracking)
        {
            db = db.AsNoTracking();
        }

        if (query.Id > 0)
        {
            db = db.Where(x => x.Id == query.Id);
        }

        return permissionRepository.FirstOrDefaultAsync(db);
    }
}