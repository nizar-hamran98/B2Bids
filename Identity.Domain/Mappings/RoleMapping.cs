using Identity.Domain.Entities;
using Identity.Domain.Models;
using SharedKernel;

namespace Identity.Domain.Mapping;
public static class RoleModelMapping
{
    public static IEnumerable<RoleModel?> ToModels(this IEnumerable<Role> entities)
    {
        return entities.Select(x => x.ToModel());
    }

    public static RoleModel? ToModel(this Role entity)
    {
        return entity == null
            ? null
            : new RoleModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Status = (EntityStatus)entity.StatusId,
                ParentId = entity.ParentId == null ? 0 : entity.ParentId,
                IsDefault = entity.IsDefault,
                RolePermissions = entity.RolePermissions?.Where(up => up.StatusId == (short)EntityStatus.Active).ToModels().Select(p => p.PermissionCode).ToList()
            };
    }

    public static Role ToEntity(this RoleModel model)
    {
        return new Role
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            StatusId = (short)model.Status,
            ParentId = model.ParentId == null ? 0 : model.ParentId,
            IsDefault = model.IsDefault
        };
    }

    public static Role? ToUpdatedEntity(this Role entity,RoleModel model)
    {
        if (entity is null) return null;

        
        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.StatusId = (short)model.Status; // 

        return entity;
    }
}
