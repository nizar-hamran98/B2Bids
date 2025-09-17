using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetUserPermissionsQuery : IQuery<List<UserPermissions>>
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long PermissionId { get; set; }
    public bool AsNoTracking { get; set; }
}

public class GetUserPermissionsQueryHandler(IRepositoryBase<UserPermissions> permissionRepository) : IQueryHandler<GetUserPermissionsQuery, List<UserPermissions>>
{
    public async Task<List<UserPermissions>> Handle(GetUserPermissionsQuery query, CancellationToken cancellationToken = default)
    {

        IQueryable<UserPermissions> db = permissionRepository.GetAll()
                .Include(x => x.Permission).Where(u => u.StatusId != (short)EntityStatus.Deleted);

        if (query.AsNoTracking)
        {
            db = db.AsNoTracking();
        }
        if (query.UserId > 0)
        {
            db = db.Where(x => x.UserId == query.UserId);
        }
        if (query.Id > 0)
        {
            db = db.Where(x => x.Id == query.Id);
        }
        if (query.PermissionId > 0)
        {
            db = db.Where(x => x.PermissionId == query.PermissionId);
        }

        return await permissionRepository.ToListAsync(db);
    }
}
