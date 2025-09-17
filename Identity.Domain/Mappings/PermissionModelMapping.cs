using Identity.Domain.Entities;
using Identity.Domain.Models;
using SharedKernel;

namespace Identity.Domain.Mapping;
public static class PermissionModelMapping
{
    public static IEnumerable<PermissionModel?> ToModels(this IEnumerable<Permissions> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static PermissionModel? ToModel(this Permissions entity)
    {
        return entity == null
            ? null
            : new PermissionModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Status = (EntityStatus)entity.StatusId,
                StatusName = entity.StatusId == 1 ? "Yes" : "No",
            };
    }

    public static Permissions ToEntity(this PermissionModel model)
    {
        return new Permissions
        {
            Id = model.Id,
            Name = model.Name,
            Code = model.Code,
            StatusId = (short)model.Status,

        };
    }
}