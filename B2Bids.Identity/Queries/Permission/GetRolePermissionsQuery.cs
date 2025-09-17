using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetRolePermissionsQuery : IQuery<List<RolePermissions>>
{
    public long Id { get; set; }
    public bool AsNoTracking { get; set; }
    public long RoleId { get; set; }
    public bool IsActive { get; set; } = false;

}

public class GetRolePermissionsQueryHandler(IRepositoryBase<RolePermissions> permissionRepository) : IQueryHandler<GetRolePermissionsQuery, List<RolePermissions>>
{
    public Task<List<RolePermissions>>? Handle(GetRolePermissionsQuery query, CancellationToken cancellationToken = default)
    {
        if (query.RoleId > 0)
        {
            IQueryable<RolePermissions> db = permissionRepository.GetAll().Include(x => x.Permission)
                .Where(u => u.StatusId != (short)EntityStatus.Deleted);

            if (query.AsNoTracking)
            {
                db = db.AsNoTracking();
            }

            if (query.Id > 0)
            {
                db = db.Where(x => x.Id == query.Id);
            }

            if (query.RoleId > 0)
            {
                db = db.Where(x => x.RoleId == query.RoleId);
            }

            if (query.IsActive)
            {
                db = db.Where(u => u.StatusId == (short)EntityStatus.Active);
            }

            return permissionRepository.ToListAsync(db);
        }
        else
        {
            return default;
        }
    }
}