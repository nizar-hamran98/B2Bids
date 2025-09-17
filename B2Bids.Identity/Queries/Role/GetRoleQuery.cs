using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetRoleQuery : IQuery<Role>
{

    public long Id { get; set; }
    public bool IncludeUsers { get; set; }
    public bool AsNoTracking { get; set; }
    public long ParentId { get; set; }
    public string Name { get; set; }
}

public class GetRoleQueryHandler(IRepositoryBase<Role> roleRepository) : IQueryHandler<GetRoleQuery, Role>
{
    public async Task<Role> Handle(GetRoleQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Role> db = roleRepository.GetAll().Where(u => u.StatusId != (short)EntityStatus.Deleted);

        if (query.IncludeUsers)
        {
            db = db.Include("UserRoles.User");
        }

        if (query.AsNoTracking)
        {
            db = db.AsNoTracking();
        }

        if (query.ParentId > 0)
        {
            db = db.Where(x => x.ParentId == query.ParentId);
        }

        if (!string.IsNullOrEmpty(query.Name))
        {
            db = db.Where(x => x.Name == query.Name);
        }
        if (query.Id > 0)
        {
            db = db.Where(x => x.Id == query.Id);
        }

        return await roleRepository.FirstOrDefaultAsync(db);
    }
}

