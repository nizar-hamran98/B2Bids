using Identity.Domain.Entities;
using Identity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class AuthenticateQuery : IQuery<User>
{
    public required AuthenticationModel entity { get; set; }
    public bool AsNoTracking { get; set; }

    public class AuthenticateQueryHandler(IRepositoryBase<User> authenticationRepository) : IQueryHandler<AuthenticateQuery, User>
    {
        public Task<User> Handle(AuthenticateQuery query, CancellationToken cancellationToken = default)
        {
            var db = authenticationRepository.GetAll()
            .Include(item => item.Role)
                .ThenInclude(item => item.RolePermissions)
                    .ThenInclude(item => item.Permission)
            .Include(item => item.UserRoles)
                .ThenInclude(item => item.Role)
                    .ThenInclude(item => item.RolePermissions)
                        .ThenInclude(item => item.Permission)
            .Include(item => item.UserPermissions)
                .ThenInclude(item => item.Permission)
            .Where(u => u.StatusId != (short)EntityStatus.Deleted);

            if (query.AsNoTracking)
            {
                db = db.AsNoTracking();
            }

            return authenticationRepository.FirstOrDefaultAsync(db.Where(x => (x.UserName == query.entity.UserName || x.Email == query.entity.UserName) && x.PasswordHash == query.entity.Password));

        }
    }
}