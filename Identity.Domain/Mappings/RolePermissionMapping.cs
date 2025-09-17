using Identity.Domain.Entities;
using Identity.Domain.Models;
using SharedKernel;

namespace Identity.Domain.Mapping;

public static class RolePermissionMapping
{
    public static IEnumerable<RolePermissionModel> ToModels(this IEnumerable<RolePermissions> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static RolePermissionModel ToModel(this RolePermissions entity)
    {
        return entity == null
            ? null
            : new RolePermissionModel
            {
                Id = entity.Id,
                PermissionId = entity.PermissionId,
                RoleId = entity.RoleId,
                Status = (EntityStatus)entity.StatusId,
                PermissionName = entity.Permission?.Name,
                PermissionCode = entity.Permission != null ? entity.Permission.Code : "",
            };
    }

    public static RolePermissions ToEntity(this RolePermissionModel model)
    {
        return new RolePermissions
        {
            Id = model.Id,
            RoleId = model.RoleId,
            PermissionId = model.PermissionId,
            StatusId = (short)model.Status,
        };
    }
}
