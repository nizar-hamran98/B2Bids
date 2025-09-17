using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetUserQuery : IQuery<User>
{
    public long Id { get; set; }
    public bool IncludeClaims { get; set; }
    public bool IncludeUserRoles { get; set; }
    public bool IncludeRoles { get; set; }
    public bool IncludeRole { get; set; } = true;
    public bool AsNoTracking { get; set; }
    public string UserName { get; set; }
    public string? Email { get; set; }
}

public class GetUserQueryHandler(IRepositoryBase<User> userRepository) : IQueryHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var db = userRepository.GetAll()
              .Where(u => u.StatusId != (short)EntityStatus.Deleted);

        if (request.IncludeRole)
        {
            db = db.Include(item => item.Role)
            .Include(x => x.Role.RolePermissions)
            .ThenInclude(x => x.Permission)
            .Include(x => x.UserPermissions)
            .ThenInclude(x => x.Permission);
        }

        if (request.IncludeUserRoles)
            db = db.Include(x => x.UserRoles);

        if (request.IncludeRoles)
            db = db.Include("UserRoles.Role");

        if (request.AsNoTracking)
            db = db.AsNoTracking();

        if (request.Id > 0)
        {
            db = db.Where(x => x.Id == request.Id);
            return await userRepository.FirstOrDefaultAsync(db);
        }

        if (!string.IsNullOrEmpty(request.UserName))
        {
            db = db.Where(x => x.UserName == request.UserName || x.Email == request.UserName);
            return await userRepository.FirstOrDefaultAsync(db);
        }
        else if (!string.IsNullOrEmpty(request.Email))
        {
            db = db.Where(x => x.Email == request.Email);
            return await userRepository.FirstOrDefaultAsync(db);
        }

        return null;
    }

}
