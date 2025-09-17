using Identity.Domain.Entities;
using MediatorCoordinator;
using MediatorCoordinator.Contract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Identity.Application.Queries;

public class GetRolesQuery : IQuery<List<Role>>
{
    public bool AsNoTracking { get; set; }
    public short StatusId { get; set; }
    public string Name { get; set; }
    public int ParentId { get; set; }
}

public class GetRolesQueryHandler(IRepositoryBase<Role> roleRepository) : IQueryHandler<GetRolesQuery, List<Role>>
{
    public async Task<List<Role>> Handle(GetRolesQuery query, CancellationToken cancellationToken = default)
    {
        IQueryable<Role> db = roleRepository.GetAll().Where(u => u.StatusId != (short)EntityStatus.Deleted);

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

        if (query.StatusId > 0)
        {
            db = db.Where(x => x.StatusId == query.StatusId);
        }

        return await roleRepository.ToListAsync(db);
    }
}
