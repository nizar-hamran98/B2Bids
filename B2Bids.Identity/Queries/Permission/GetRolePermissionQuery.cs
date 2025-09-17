using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetRolePermissionQuery : IQuery<RolePermissions>
{
    public long Id { get; set; }
    public bool AsNoTracking { get; set; }
    public long RoleId { get; set; }
    public long PermissionId { get; set; }

}

public class GetRolePermissionQueryHandler(IRepositoryBase<RolePermissions> rolePermissionRepository) : IQueryHandler<GetRolePermissionQuery, RolePermissions>
{
    public Task<RolePermissions> Handle(GetRolePermissionQuery query, CancellationToken cancellationToken = default)
    {

        IQueryable<RolePermissions> db = rolePermissionRepository.GetAll().Where(u => u.StatusId != (short)EntityStatus.Deleted);
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
        if (query.PermissionId > 0)
        {
            db = db.Where(x => x.PermissionId == query.PermissionId);
        }

        return rolePermissionRepository.FirstOrDefaultAsync(db);
    }
}