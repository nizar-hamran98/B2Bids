using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetUserPermissionsByUserIdQuery : IQuery<List<UserPermissions>>
{
    public bool AsNoTracking { get; set; }
    public long UserId { get; set; }
    public long PermissionId { get; set; }


}

public class GetUserPermissionsByUserIdQueryHandler(IRepositoryBase<UserPermissions> repository) : IQueryHandler<GetUserPermissionsByUserIdQuery, List<UserPermissions>>
{
    public async Task<List<UserPermissions>>? Handle(GetUserPermissionsByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        if (query.UserId > 0)
        {
            IQueryable<UserPermissions> db = repository.GetAll()
                .Include(x => x.Permission).Where(u => u.StatusId != (short)EntityStatus.Deleted);

            if (query.AsNoTracking)
            {
                db = db.AsNoTracking();
            }
            if (query.UserId > 0)
            {
                db = db.Where(x => x.UserId == query.UserId);
            }
            if (query.PermissionId > 0)
            {
                db = db.Where(x => x.PermissionId == query.PermissionId);
            }

            return await repository.ToListAsync(db);
        }
        else
        {
            return default;
        }
    }
}
