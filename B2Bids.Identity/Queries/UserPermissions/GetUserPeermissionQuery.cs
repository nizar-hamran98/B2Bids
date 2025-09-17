using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetUserPermissionQuery : IQuery<UserPermissions>
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long PermissionId { get; set; }
    public bool AsNoTracking { get; set; }
}

public class GetUserPermissionQueryHandler(IRepositoryBase<UserPermissions> repository) : IQueryHandler<GetUserPermissionQuery, UserPermissions>
{
    public async Task<UserPermissions> Handle(GetUserPermissionQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<UserPermissions> db = repository.GetAll()
            .Include(x => x.Permission).Where(u => u.StatusId != (short)EntityStatus.Deleted);

        if (query.AsNoTracking)
            db = db.AsNoTracking();
        if (query.Id > 0) 
        { 
            db = db.Where(x => x.Id == query.Id);
        return await repository.FirstOrDefaultAsync(db);
        
        }
        if (query.UserId > 0 && query.PermissionId > 0)
            db = db.Where(x => x.PermissionId == query.PermissionId && x.UserId == query.UserId);

            return await repository.FirstOrDefaultAsync(db);
        

    }
}