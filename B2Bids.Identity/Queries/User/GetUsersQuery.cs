using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetUsersQuery : IQuery<List<User>>
{
   
    public bool IncludeUserRoles { get; set; }
    public bool IncludeRoles { get; set; }
    public bool AsNoTracking { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
  
}
public class GetUsersQueryHandler(IRepositoryBase<User> userRepository, IUnitOfWork unitOfWork) : IQueryHandler<GetUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        IQueryable<User> db = userRepository.GetAll().Include(item => item.Role).Where(u => u.StatusId != (short)EntityStatus.Deleted);

        if (query.IncludeUserRoles)
            db = db.Include(x => x.UserRoles);
        
        if (query.IncludeRoles)
            db = db.Include("UserRoles.Role");

        if (query.AsNoTracking)
            db = db.AsNoTracking();

        if (!string.IsNullOrEmpty(query.UserName))
            db = db.Where(x => x.UserName == query.UserName);

        if (!string.IsNullOrEmpty(query.Email))
            db = db.Where(x => x.Email == query.Email);

        return await userRepository.ToListAsync(db);
    }
}
